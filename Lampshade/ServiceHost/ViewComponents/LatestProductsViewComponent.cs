using _01_LampshadeQuery.Contracts.Prodcut;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class LatestProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public LatestProductsViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var latestProducts = _productQuery.GetLatestProducts();
            return View("_LatestProducts", latestProducts);
        }
    }
}
