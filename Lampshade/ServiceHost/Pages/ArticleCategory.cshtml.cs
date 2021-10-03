using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ServiceHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> latestArticle;

        private readonly IArticleCategoryQuery _articleCategory;
        private readonly IArticleQuery _article;

        public ArticleCategoryModel(IArticleCategoryQuery articleCategory, IArticleQuery article)
        {
            _articleCategory = articleCategory;
            _article = article;
        }

        public void OnGet(string id)
        {
            ArticleCategory = _articleCategory.GetArticleCategory(id);
            ArticleCategories = _articleCategory.GetArticleCategories();
            latestArticle = _article.LatestArticles();
        }
    }
}
