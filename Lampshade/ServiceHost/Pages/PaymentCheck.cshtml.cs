using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery.Contracts;
using _01_LampshadeQuery.Contracts.Prodcut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    [Authorize]
    public class PaymentCheckModel : PageModel
    {
        public Cart cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper _authHelper;
        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly IZarinPalFactory _zarinPalFactroy;
        private readonly IOrderApplication _orderApplication;
        private readonly ICartCalculateService _cartCalculateService;

        public PaymentCheckModel(ICartCalculateService cartCalculateService, ICartService cartService,
            IProductQuery productQuery, IZarinPalFactory zarinPalFactroy, IOrderApplication orderApplication, IAuthHelper authHelper)
        {
            _cartCalculateService = cartCalculateService;
            _cartService = cartService;
            _productQuery = productQuery;
            _zarinPalFactroy = zarinPalFactroy;
            _orderApplication = orderApplication;
            _authHelper = authHelper;
            cart = new Cart();
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            cartItems.ForEach(x => x.CalculateTotalItemPrice());
            cart = _cartCalculateService.ComputeCart(cartItems);

            _cartService.Set(cart);
        }

        public IActionResult OnPostPay(int paymentMethod)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);
            var result = _productQuery.CheckInventoryStatus(cart.Items);
            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            var orderId = _orderApplication.PlaceOrder(cart);

            var accountUsername = _authHelper.CurrentAccountInfo().Username;
            var accountMobile = _authHelper.CurrentAccountMobile();
            if (paymentMethod == 1)
            {
                var paymentResponse = _zarinPalFactroy.CreatePaymentRequest(cart.PayAmount.ToString(), accountMobile, accountUsername+"@yhaoo.com",
                "خرید از فروشگاه لوازم روشنایی و دکوری", orderId);

                return Redirect($"https://{_zarinPalFactroy.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
            }
            else
            {
                var paymentResult = new PaymentResult();
                return RedirectToPage("/PaymentResult", paymentResult.Succeeded(
                    "سفارش شما با موفقیت ثبت شد.پس از تماس کارشناسان ما و پرداخت وجه، سفارش شما ارسال خواهد شد."
                    , ""));
            }
        }

        public RedirectToPageResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] long oId)
        {
            var orderAmount = _orderApplication.GetAmountBy(oId);
            var verficationResponse = _zarinPalFactroy.CreateVerificationRequest(authority, orderAmount.ToString(CultureInfo.InvariantCulture));

            var result = new PaymentResult();
            if (status.ToLower() == "ok" && verficationResponse.Status == 100)
            {
                var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verficationResponse.RefID);
                Response.Cookies.Delete("cart-items");

                result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);

                return RedirectToPage("/PaymentResult", result);
            }

            result = result.Failed("پرداخت با موفقیت انجام نشد. در صورت کسر از حساب تا 24 ساعت آینده به حساب شما واریز می شود.");
            return RedirectToPage("/PaymentResult", result);
        }
    }
}
