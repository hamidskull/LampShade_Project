using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.Orders
{
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel { get; set; }
        public List<OrderViewModel> orderList;
        public SelectList Accounts;

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;
        public IndexModel(IOrderApplication orderApplication, IAccountApplication accountApplication)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
        }
        public void OnGet(OrderSearchModel searchModel)
        {
            Accounts = new SelectList(_accountApplication.GetAccounts(), "Id", "Fullname");
            orderList = _orderApplication.Search(searchModel);
        }

        public RedirectToPageResult OnGetConfirm(long id)
        {
            _orderApplication.PaymentSucceeded(id, 0);
            return RedirectToPage("./Index");
        }

        public RedirectToPageResult OnGetCancel(long id)
        {
            _orderApplication.Cancel(id);
            return RedirectToPage("./Index");
        }

        public PartialViewResult OnGetItems(long id)
        {
            var items = _orderApplication.GetOrderItems(id);
            return Partial("Items", items);
        }
    }
}
