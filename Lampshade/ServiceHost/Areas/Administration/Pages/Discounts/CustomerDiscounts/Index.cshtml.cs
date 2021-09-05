using DiscountManagement.Application.Contracts.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscounts
{
    public class IndexModel : PageModel
    {
        public CustomerDiscountSearchModel SearchModel { get; set; }
        public SelectList productList;
        public List<CustomerDiscountViewModel> customerDiscounts;

        private readonly IProductApplication _productApplication;
        private readonly ICustomerDiscountApplication _customerDiscountApplication;
        public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
        }

        public void OnGet(CustomerDiscountSearchModel searchModel)
        {
            customerDiscounts = _customerDiscountApplication.Search(searchModel);
            productList = new SelectList(_productApplication.GetProduct(), "Id", "Name");

        }

        public PartialViewResult OnGetCreate()
        {
            var command = new CreateCustomerDiscount
            {
                Products = _productApplication.GetProduct()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var discount = _customerDiscountApplication.GetDetails(id);
            discount.Products = _productApplication.GetProduct();

            return Partial("./Edit", discount);
        }
        public JsonResult OnPostEdit(EditCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
