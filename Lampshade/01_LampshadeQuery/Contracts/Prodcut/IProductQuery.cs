using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts.Prodcut
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestProducts();
        ProductQueryModel GetDetails(string slug);
        List<ProductQueryModel> Search(string value);
    }
}
