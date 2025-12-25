using Dev.Module.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Auth.Infrastructure.Persistence.Configurations;

public class TenantConfig : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(Tenant)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasMaxLength(25);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.PrimaryDomain).HasMaxLength(50);
    }
}
