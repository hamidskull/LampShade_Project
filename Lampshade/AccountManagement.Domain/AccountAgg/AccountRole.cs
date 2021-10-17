using _0_Framework.Domain;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Domain.AccountAgg
{
    public class AccountRole : EntityBase
    {
        public long AccountId { get; private set; }
        public long RoleId { get; private set; }

        public Account Account { get; private set; }
        public Role Role { get; private set; }


        protected AccountRole() { }

        public AccountRole(long accountId, long roleId)
        {
            AccountId = accountId;
            RoleId = roleId;
        }
    }
}
