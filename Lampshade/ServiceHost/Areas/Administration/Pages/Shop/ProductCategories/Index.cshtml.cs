using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductCategories
{
    public class IndexModel : PageModel
    {
        public ProductCategorySearchModel SearchModel { get; set; }
        public List<ProductCategoryViewModel> productCategoryList;
        private readonly IProductCategoryApplication _prodcutCategoryApplication;
        public IndexModel(IProductCategoryApplication prodcutCategoryApplication)
        {
            _prodcutCategoryApplication = prodcutCategoryApplication;
        }

        [NeedsPermissions(ShopPermissions.ListProductCategories)]
        public void OnGet(ProductCategorySearchModel searchModel)
        {
            productCategoryList = _prodcutCategoryApplication.Search(searchModel);
        }

        public PartialViewResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        [NeedsPermissions(ShopPermissions.CreateProductCategories)]
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

        [NeedsPermissions(ShopPermissions.EditProductCategories)]
        public JsonResult OnPostEdit(EditProductCategory command)
        {
            var result = _prodcutCategoryApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
