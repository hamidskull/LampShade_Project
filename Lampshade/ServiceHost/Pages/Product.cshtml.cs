using _01_LampshadeQuery.Contracts.Prodcut;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            product = _productQuery.GetProductDetails(id);
        }
        public RedirectToPageResult OnPost(CreateComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = _commentApplication.Create(command);
            return RedirectToPage("/Product", new { Id = productSlug });
        }
    }
}
