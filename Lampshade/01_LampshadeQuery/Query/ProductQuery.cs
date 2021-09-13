using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Prodcut;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampshadeQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopContext _context;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        public ProductQuery(ShopContext context,
            InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductQueryModel> GetLatestProducts()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();
            var productDiscountRate = _discountContext.CustomerDiscounts.Select(x => new { x.ProductId, x.DiscountRate }).ToList();

            var products = _context.Products.Include(x => x.Category).Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).OrderByDescending(x => x.Id).Take(6).ToList();

            products.ForEach(p =>
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
            });

            return products;
        }
    }
}
