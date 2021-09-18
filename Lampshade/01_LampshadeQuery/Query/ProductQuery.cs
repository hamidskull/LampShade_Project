using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Prodcut;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductPictureAgg;
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

        public ProductQueryModel GetDetails(string slug)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice, x.InStock }).ToList();
            var productDiscountRate = _discountContext.CustomerDiscounts.Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();

            var product = _context.Products.Include(x => x.Category).Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                CategorySlug = x.Category.Slug,
                Code = x.Code,
                Description = x.Code,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                ShortDescription = x.ShortDescription,
                Pictures = MapProductPictures(x.ProductPictures)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

            if (product == null)
                return new ProductQueryModel();

            var inventoyProduct = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (inventoyProduct != null)
            {
                product.IsInStock = inventoyProduct.InStock;
                var price = inventoyProduct.UnitPrice;
                product.Price = price.ToMoney();

                var productDisRate = productDiscountRate.FirstOrDefault(x => x.ProductId == product.Id);
                if (productDisRate != null)
                {
                    var discountRate = productDisRate.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = productDisRate.EndDate.ToDiscountFormat();

                    product.HasDiscount = product.DiscountRate > 0;

                    var discountAmount = Math.Round((price * discountRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }

            return product;
        }

        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> productPictures)
        {
            return productPictures.Select(x => new ProductPictureQueryModel
            {
                IsRemove = x.IsRemoved,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemove).ToList();
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
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(6).ToList();

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

        public List<ProductQueryModel> Search(string value)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();
            var productDiscountRate = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();

            var query = _context.Products
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription
                }).AsNoTracking();

            if (!string.IsNullOrEmpty(value))
                query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));

            var products = query.OrderByDescending(x => x.Id).ToList();

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
                        p.DiscountExpireDate = productDisRate.EndDate.ToDiscountFormat();

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
