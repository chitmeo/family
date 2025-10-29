using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class JournalEntryLineConfig : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(JournalEntryLine)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Description).HasMaxLength(255);

        builder.HasOne(jel => jel.JournalEntry)
               .WithMany(je => je.JournalEntryLines)
               .HasForeignKey(pk => pk.JournalEntryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}