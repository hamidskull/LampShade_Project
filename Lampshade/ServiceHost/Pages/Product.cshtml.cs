using _01_LampshadeQuery.Contracts.Prodcut;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Comment;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        public ProductQueryModel product;
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;

        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }

        public void OnGet(string id)
        {
            product = _productQuery.GetDetails(id);
        }
        public RedirectToPageResult OnPost(CreateComment command, string productSlug)
        {
            var result = _commentApplication.Create(command);
            return RedirectToPage("/Product", new { Id = productSlug });
        }
    }
}
