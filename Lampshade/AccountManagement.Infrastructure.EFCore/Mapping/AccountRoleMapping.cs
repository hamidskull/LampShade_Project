using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class AccountRoleMapping //: IEntityTypeConfiguration<AccountRole>
    {
        //public void Configure(EntityTypeBuilder<AccountRole> builder)
        //{
        //    //builder.ToTable("AccountRoles");
        //    //builder.HasKey(x => x.Id);

        //    //builder.HasOne(x => x.Account).WithMany(x => x.AccountRoles).HasForeignKey(x => x.AccountId);
        //    //builder.HasOne(x => x.Role).WithMany(x => x.AccountRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction);
        //}
    }
}
