using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        ProductCategoryQueryModel GetProductCategoryProductsBy(string slug);
        List<ProductCategoryQueryModel> GetProductCategoryList();
        List<ProductCategoryQueryModel> GetProductCategoreisWithProductList();
    }
}
