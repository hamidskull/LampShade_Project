using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        public ProductSearchModel SearchModel { get; set; }
        public SelectList productCategoryList;
        public List<ProductViewModel> productList;

        private readonly IProductCategoryApplication _prodcutCategoryApplication;
        private readonly IProductApplication _productApplication;
        public IndexModel(IProductCategoryApplication prodcutCategoryApplication, IProductApplication productApplication)
        {
            _prodcutCategoryApplication = prodcutCategoryApplication;
            _productApplication = productApplication;
        }

        public void OnGet(ProductSearchModel searchModel)
        {
            productList = _productApplication.Search(searchModel);
            productCategoryList = new SelectList(_prodcutCategoryApplication.GetProductCategories(), "Id", "Name");

        }

        public PartialViewResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = _prodcutCategoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProduct command)
        {
            var result = _productApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var product = _productApplication.GetDetails(id);
            product.Categories = _prodcutCategoryApplication.GetProductCategories();

            return Partial("./Edit", product);
        }
        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = _productApplication.Edit(command);
            return new JsonResult(result);
        }
        public RedirectToPageResult OnGetNotInStock(long id)
        {
            _productApplication.NotInStock(id);
            return RedirectToPage("./Index");
        }
        public RedirectToPageResult OnGetIsInStock(long id)
        {
            _productApplication.InStock(id);
            return RedirectToPage("./Index");
        }
    }
}
