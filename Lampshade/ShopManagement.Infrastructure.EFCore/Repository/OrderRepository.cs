using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using System.Collections.Generic;
using System.Linq;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
    {
        private readonly ShopContext _context;
        private readonly AccountContext _accountContext;

        public OrderRepository(ShopContext context, AccountContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
        }

        public double GetAmountBy(long id)
        {
            var order = _context.Orders.Select(x => new { x.PayAmount, x.Id }).FirstOrDefault(x => x.Id == id);
            if (order != null)
                return order.PayAmount;

            return 0;

        }

        public List<OrderItemViewModel> GetOrderItems(long orderId)
        {
            var products = _context.Products.Select(x => new { x.Id, x.Name }).ToList();
            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
                return new List<OrderItemViewModel>();

            var items = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                OrderId = orderId,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).ToList();

            foreach (var item in items)
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId).Name;

            return items;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var account = _accountContext.Accounts.Select(x => new { x.Id, x.Fullname }).ToList();

            var query = _context.Orders.Select(x => new OrderViewModel
            {
                AccountId = x.AccountId,
                CreationDate = x.CreationDate.ToFarsi(),
                DiscountAmount = x.DiscountAmount,
                Id = x.Id,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                // AccountFullname = account.FirstOrDefault(z => z.Id == x.AccountId).Fullname,
                //PaymentMethod = PaymentMethod.GetBy(x.PaymentMethod).Name
            }).AsNoTracking();

            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);

            var orders = query.OrderByDescending(x => x.Id).ToList();

            foreach (var item in orders)
            {
                item.AccountFullname = account.FirstOrDefault(x => x.Id == item.AccountId).Fullname;
                item.PaymentMethod = PaymentMethod.GetBy(item.PaymentMethodId).Name;
            }

            return orders;
        }
    }
}
