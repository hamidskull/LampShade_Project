using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using ShopManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Linq;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository : RepositoryBase<long, CustomerDiscount>, ICustomerDiscountRepository
    {
        private readonly DiscountContext _context;
        private readonly ShopContext _shopContext;
        public CustomerDiscountRepository(DiscountContext context, ShopContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public List<CustomerDiscountViewModel> GetAll()
        {
            return _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
            {
                DiscountRate = x.DiscountRate,
                Id = x.Id,
                EndDate = x.EndDate.ToFarsi(),
                ProductId = x.ProductId,
                Reason = x.Reason,
                StartDate = x.StartDate.ToFarsi()
            }).ToList();
        }

        public EditCustomerDiscount GetDetails(long id)
        {
            return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount
            {
                DiscountRate = x.DiscountRate,
                EndDate = x.EndDate.ToFarsi(),
                StartDate = x.StartDate.ToFarsi(),
                Reason = x.Reason,
                ProductId = x.ProductId,
                Id = x.Id
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var query = _context.CustomerDiscounts.Select(x => new CustomerDiscountViewModel
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId,
                Reason = x.Reason,
                StartDate = x.StartDate.ToFarsi(),
                EndDate = x.EndDate.ToFarsi(),
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (!string.IsNullOrEmpty(searchModel.StartDate))
                //query = query.Where(x => Convert.ToDateTime(x.StartDate) < Convert.ToDateTime(searchModel.StartDate));
                query = query.Where(x => x.StartDate.ToGeorgianDateTime() < searchModel.StartDate.ToGeorgianDateTime());

            if (!string.IsNullOrEmpty(searchModel.EndDate))
                //query = query.Where(x => Convert.ToDateTime(x.EndDate) > Convert.ToDateTime(searchModel.EndDate));
                query = query.Where(x => x.EndDate.ToGeorgianDateTime() > searchModel.EndDate.ToGeorgianDateTime());

            var discounts = query.OrderByDescending(x => x.Id).ToList();
            discounts.ForEach(dis => dis.Product = products.FirstOrDefault(x => x.Id == dis.ProductId).Name);
            return discounts;

            // return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
