namespace ShopManagement.Domain.Services
{
    public interface IShopAccountACL
    {
        (string fullname, string mobile) GetAccountBy(long id);
    }
}
