using System.Collections.Generic;
using _01_LampshadeQuery.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class PaymentCheckModel : PageModel
    {
        public Cart cart;
        public const string CookieName = "cart-items";
        private readonly ICartCalculateService _cartCalculateService;

        public PaymentCheckModel(ICartCalculateService cartCalculateService)
        {
            _cartCalculateService = cartCalculateService;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            cartItems.ForEach(x => x.CalculateTotalItemPrice());
            cart = _cartCalculateService.ComputeCart(cartItems);
        }
    }
}
