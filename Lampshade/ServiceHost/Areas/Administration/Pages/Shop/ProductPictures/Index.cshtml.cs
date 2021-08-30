using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductPictures
{
    public class IndexModel : PageModel
    {
        public ProductPictureSearchModel SearchModel { get; set; }
        public SelectList productList;
        public List<ProductPictureViewModel> productPictureList;

        private readonly IProductApplication _prodcutApplication;
        private readonly IProductPictureApplication _productPictureApplication;
        public IndexModel(IProductApplication prodcutApplication, IProductPictureApplication productPictureApplication)
        {
            _prodcutApplication = prodcutApplication;
            _productPictureApplication = productPictureApplication;
        }

        public void OnGet(ProductPictureSearchModel searchModel)
        {
            productPictureList = _productPictureApplication.Search(searchModel);
            productList = new SelectList(_prodcutApplication.GetProduct(), "Id", "Name");
        }

        public PartialViewResult OnGetCreate()
        {
            var command = new CreateProductPicture
            {
                Products = _prodcutApplication.GetProduct()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProductPicture command)
        {
            var result = _productPictureApplication.Create(command);
            return new JsonResult(result);
        }
        public PartialViewResult OnGetEdit(long id)
        {
            var productPicture = _productPictureApplication.GetDetails(id);
            productPicture.Products = _prodcutApplication.GetProduct();

            return Partial("./Edit", productPicture);
        }
        public JsonResult OnPostEdit(EditProductPicture command)
        {
            var result = _productPictureApplication.Edit(command);
            return new JsonResult(result);
        }
        public RedirectToPageResult OnGetRemove(long id)
        {
            _productPictureApplication.Remove(id);
            return RedirectToPage("./Index");
        }
        public RedirectToPageResult OnGetRestore(long id)
        {
            _productPictureApplication.Restore(id);
            return RedirectToPage("./Index");
        }
    }
}
