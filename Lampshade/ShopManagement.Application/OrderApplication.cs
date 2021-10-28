using _0_Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IShopInventoryACL _shopInventoryACL;

        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper,
            IConfiguration configuration, IShopInventoryACL shopInventoryACL)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _shopInventoryACL = shopInventoryACL;
        }

        public void Cancel(long id)
        {
            var order = _orderRepository.Get(id);
            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public double GetAmountBy(long id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public List<OrderItemViewModel> GetOrderItems(long orderId)
        {
            return _orderRepository.GetOrderItems(orderId);
        }

        public string PaymentSucceeded(long orderId, long refId)
        {
            var order = _orderRepository.Get(orderId);
            order.PaymentSucceded(refId);

            var symbol = _configuration["Symbol"];
            var issueTrackingNo = CodeGenerator.Generate(symbol);
            order.SetIssueTrackingNo(issueTrackingNo);
            
            if (!_shopInventoryACL.ReduceFromInventory(order.Items)) return "";
            
            _orderRepository.SaveChanges();
            return issueTrackingNo;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();

            //var symbol = _configuration["Symbol"];
            //var issueTrackingNo = CodeGenerator.Generate(symbol);

            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount);
            cart.Items.ForEach(cartItem =>
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            });

            _orderRepository.Create(order);
            _orderRepository.SaveChanges();

            return order.Id;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }
    }
}
