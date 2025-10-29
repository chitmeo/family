using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Auth.Infrastructure.Persistence.Configurations;

internal class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(Role)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(255);
        builder.Property(x => x.SystemName).HasMaxLength(255);
    }
}
