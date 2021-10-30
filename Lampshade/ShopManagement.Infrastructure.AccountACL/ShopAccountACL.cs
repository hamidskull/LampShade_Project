using AccountManagement.Application.Contracts.Account;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.AccountACL
{
    public class ShopAccountACL : IShopAccountACL
    {
        private readonly IAccountApplication _accountApplication;

        public ShopAccountACL(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        (string fullname, string mobile) IShopAccountACL.GetAccountBy(long id)
        {
            var account = _accountApplication.GetAccountBy(id);

            return (account.Fullname, account.Mobile);
        }
    }
}
