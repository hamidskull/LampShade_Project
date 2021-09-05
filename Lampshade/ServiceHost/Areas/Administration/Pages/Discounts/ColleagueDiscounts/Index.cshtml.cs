using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Discounts.ColleagueDiscounts
{
    public class IndexModel : PageModel
    {
        public ColleagueDiscountSearchModel SearchModel { get; set; }
        public SelectList productList;
        public List<ColleagueDiscountViewModel> colleagueDiscounts;

        private readonly IProductApplication _productApplication;
        private readonly IColleagueDiscountApplication _colleagueDiscountApplication;
        public IndexModel(IProductApplication productApplication, IColleagueDiscountApplication colleagueDiscountApplication)
        {
            _productApplication = productApplication;
            _colleagueDiscountApplication = colleagueDiscountApplication;
        }

        public void OnGet(ColleagueDiscountSearchModel searchModel)
        {
            colleagueDiscounts = _colleagueDiscountApplication.Search(searchModel);
            productList = new SelectList(_productApplication.GetProduct(), "Id", "Name");

        }

        public PartialViewResult OnGetCreate()
        {
            var command = new CreateColleagueDiscount
            {
                Products = _productApplication.GetProduct()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateColleagueDiscount command)
        {
            var result = _colleagueDiscountApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var discount = _colleagueDiscountApplication.GetDetails(id);
            discount.Products = _productApplication.GetProduct();

            return Partial("./Edit", discount);
        }
        public JsonResult OnPostEdit(EditColleagueDiscount command)
        {
            var result = _colleagueDiscountApplication.Edit(command);
            return new JsonResult(result);
        }
        public RedirectToPageResult OnGetRestore(long id)
        {
            _colleagueDiscountApplication.Restore(id);
            return RedirectToPage("./Index");
        }
        public RedirectToPageResult OnGetRemove(long id)
        {
            _colleagueDiscountApplication.Remove(id);
            return RedirectToPage("./Index");
        }
    }
}
