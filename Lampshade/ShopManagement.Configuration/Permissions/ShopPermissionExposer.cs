using _0_Framework.Infrastructure;
using System.Collections.Generic;

namespace ShopManagement.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDTO>> Expose()
        {
            return new Dictionary<string, List<PermissionDTO>>
            {
                { "Product",new List<PermissionDTO>
                     {
                    new PermissionDTO(ShopPermissions.ListProduct,"ListProducts"),
                    new PermissionDTO(ShopPermissions.SearchProduct,"SearchProducts"),
                    new PermissionDTO(ShopPermissions.CreateProduct,"CreateProducts"),
                    new PermissionDTO(ShopPermissions.EditProduct,"EditProducts")
                     }
                },
                {
                 "ProductCategory", new List<PermissionDTO>
                 {
                    new PermissionDTO(ShopPermissions.ListProductCategories,"ListProductCategories"),
                    new PermissionDTO(ShopPermissions.SearchProductCategories,"SearchProductCategories"),
                    new PermissionDTO(ShopPermissions.CreateProductCategories,"CreateProductCategory"),
                    new PermissionDTO(ShopPermissions.EditProductCategories,"EditProductCategory")
                                                                                         }
                }
        };
        }
    }
}
