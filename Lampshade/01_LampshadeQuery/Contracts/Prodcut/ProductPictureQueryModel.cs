namespace _01_LampshadeQuery.Contracts.Prodcut
{
    public class ProductPictureQueryModel
    {
        public long ProductId { get; set; }
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public bool IsRemove { get; set; }
    }
}
