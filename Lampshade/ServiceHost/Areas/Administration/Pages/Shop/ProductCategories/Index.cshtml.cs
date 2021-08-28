using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductCategories
{
    public class IndexModel : PageModel
    {
        public ProductCategorySearchModel SearchModel { get; set; }
        public List<ProductCategoryViewModel> productCategoryList;
        private readonly IProdcutCategoryApplication _prodcutCategoryApplication;
        public IndexModel(IProdcutCategoryApplication prodcutCategoryApplication)
        {
            _prodcutCategoryApplication = prodcutCategoryApplication;
        }

        public void OnGet(ProductCategorySearchModel searchModel)
        {
            productCategoryList = _prodcutCategoryApplication.Search(searchModel);
        }

        public PartialViewResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        public JsonResult OnPostCreate(CreateProductCategory command)
        {
            var result = _prodcutCategoryApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var productCategory = _prodcutCategoryApplication.GetDetails(id);
            return Partial("./Edit", productCategory);
        }
        public JsonResult OnPostEdit(EditProductCategory command)
        {
            var result = _prodcutCategoryApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
