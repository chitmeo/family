using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class JournalConfig : IEntityTypeConfiguration<JournalTemplate>
{
    public void Configure(EntityTypeBuilder<JournalTemplate> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(JournalTemplate)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Type).HasMaxLength(10);
        builder.Property(x => x.Description).HasMaxLength(255);
    }
}
