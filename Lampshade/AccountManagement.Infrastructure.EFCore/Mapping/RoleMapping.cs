using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EFCore.Mapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Ignore(x => x.AccountRoles);

            builder.HasMany(x => x.Accounts).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);

            builder.OwnsMany(x => x.Permissions, modelBuilder =>
            {
                modelBuilder.ToTable("RolePermissions");
                modelBuilder.HasKey(x => x.Id);
                modelBuilder.Ignore(x => x.Name);
                modelBuilder.WithOwner(x => x.Role).HasForeignKey(x => x.RoleId);
            });

            //builder.OwnsMany(x => x.AccountRoles, model =>
            //{
            //    model.ToTable("UserRoles");
            //    model.HasKey(x => x.RoleId);
            //    model.WithOwner(x => x.Role).HasForeignKey(x => x.RoleId);

            //});

            ////add for many roles
            //builder.HasMany(x => x.AccountRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        }
    }
}
