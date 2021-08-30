using _0_Framework.Domain;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using System.Collections.Generic;

namespace ShopManagement.Domain.ProductAgg
{
    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public double UnitPrice { get; private set; }
        public bool IsInStock { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public string Slug { get; private set; }
        public string MetaDescription { get; private set; }
        public string Keywords { get; private set; }
        public long CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }
        public List<ProductPicture> ProductPictures { get; private set; }

        protected Product() { }

        public Product(string name, string code, double unitPrice, string shortDescription, string description,
            string picture, string pictureAlt, string pictureTitle, string slug, string metaDescription,
            string keyWords, long categoryId)
        {
            Name = name;
            Code = code;
            UnitPrice = unitPrice;
            ShortDescription = shortDescription;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Slug = slug;
            MetaDescription = metaDescription;
            Keywords = keyWords;
            CategoryId = categoryId;
            IsInStock = true;
            ProductPictures = new List<ProductPicture>();
        }
        public void Edit(string name, string code, double unitPrice, string shortDescription, string description,
            string picture, string pictureAlt, string pictureTitle, string slug, string metaDescription,
            string keyWords, long categoryId)
        {
            Name = name;
            Code = code;
            UnitPrice = unitPrice;
            ShortDescription = shortDescription;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Slug = slug;
            MetaDescription = metaDescription;
            Keywords = keyWords;
            CategoryId = categoryId;
        }
        public void InStock()
        {
            IsInStock = true;
        }
        public void NotInStock()
        {
            IsInStock = false;
        }
    }
}
