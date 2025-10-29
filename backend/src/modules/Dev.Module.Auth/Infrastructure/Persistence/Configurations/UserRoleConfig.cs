using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Auth.Infrastructure.Persistence.Configurations;

internal class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public static void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(UserRole)}");
        builder.HasKey(x=> new { x.UserId, x.RoleId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    void IEntityTypeConfiguration<UserRole>.Configure(EntityTypeBuilder<UserRole> builder)
    {
        Configure(builder);
    }
}
