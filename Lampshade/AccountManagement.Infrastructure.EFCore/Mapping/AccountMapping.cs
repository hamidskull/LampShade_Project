using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Fullname).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ProfilePhoto).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Mobile).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();

            builder.HasOne(x => x.Role).WithMany(x => x.Accounts).HasForeignKey(x => x.RoleId);


            builder.OwnsMany(x => x.AccountRoles, model =>
              {
                  model.ToTable("UserRoles");
                  model.HasKey(x => x.Id);
                  model.WithOwner(x => x.Account).HasForeignKey(x => x.AccountId);
              });

            ////add for many roles
            //builder.HasMany(x => x.AccountRoles).WithOne(x => x.Account).HasForeignKey(x => x.AccountId);
        }
    }
}
