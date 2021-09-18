using _0_Framework.Domain;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Domain.ProductPictureAgg
{
    public class ProductPicture : EntityBase
    {
        public long ProductId { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }
        public bool IsRemoved { get; private set; }
        public Product Product { get; private set; }

        protected ProductPicture() { }

        public ProductPicture(long porductId, string picture, string pictureAlt, string pictureTitle)
        {
            ProductId = porductId;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            IsRemoved = false;
        }
        public void Edit(long porductId, string picture, string pictureAlt, string pictureTitle)
        {
            ProductId = porductId;
            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
        }
        public void Remove()
        {
            IsRemoved = true;
        }
        public void Restrore()
        {
            IsRemoved = false;
        }
    }
}
