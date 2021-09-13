using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Prodcut;
using _01_LampshadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampshadeQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        public ProductCategoryQuery(ShopContext context,
            InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductCategoryQueryModel> GetProductCategoreisWithProductList()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();
            var productDiscountRate = _discountContext.CustomerDiscounts.Select(x => new { x.ProductId, x.DiscountRate }).ToList();

            var categories = _context.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Products = MapProducts(x.Products)
                }).ToList();

            //categories.ForEach(cat => cat.Products.ForEach
            //(p=>p.Price = inventory.FirstOrDefault(x=>x.ProductId==p.Id)?.UnitPrice.ToMoney()));

            categories.ForEach(cat => cat.Products.ForEach(p =>
            {
                var inventoyProduct = inventory.FirstOrDefault(x => x.ProductId == p.Id);
                if (inventoyProduct != null)
                {
                    var price = inventoyProduct.UnitPrice;
                    p.Price = price.ToMoney();

                    var productDisRate = productDiscountRate.FirstOrDefault(x => x.ProductId == p.Id);
                    if (productDisRate != null)
                    {
                        var discountRate = productDisRate.DiscountRate;
                        p.DiscountRate = discountRate;

                        p.HasDiscount = p.DiscountRate > 0;

                        var discountAmount = Math.Round((price * discountRate) / 100);
                        p.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }));

            return categories;
        }

        private static List<ProductQueryModel> MapProducts(List<Product> products)
        {
            return products.Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).OrderByDescending(x => x.Id).ToList();
            //var result = new List<ProductQueryModel>();
            //foreach (var product in products)
            //{
            //    var item = new ProductQueryModel
            //    {
            //        Id = product.Id,
            //        Category = product.Category.Name,
            //        Name = product.Name,
            //        Picture = product.Picture,
            //        PictureAlt = product.PictureAlt,
            //        PictureTitle = product.PictureTitle,
            //        Slug = product.Slug,
            //        Description = product.Description
            //    };
            //    result.Add(item);
            //}

            //return result;
        }

        public List<ProductCategoryQueryModel> GetProductCategoryList()
        {
            return _context.ProductCategories.Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).ToList();
        }
    }
}
