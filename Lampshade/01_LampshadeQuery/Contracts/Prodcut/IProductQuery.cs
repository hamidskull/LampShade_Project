using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts.Prodcut
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestProducts();
    }
}
