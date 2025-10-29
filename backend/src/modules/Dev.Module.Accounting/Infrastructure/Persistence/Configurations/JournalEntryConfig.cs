using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class JournalEntryConfig : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(JournalEntry)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Reference).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(255);

        builder.HasOne(je => je.Journal)
               .WithMany(j => j.JournalEntries)
               .HasForeignKey(pk => pk.JournalId)
               .OnDelete(DeleteBehavior.Restrict);        
    }
}
