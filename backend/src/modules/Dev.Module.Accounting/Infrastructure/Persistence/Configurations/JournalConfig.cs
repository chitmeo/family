using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class JournalConfig : IEntityTypeConfiguration<Journal>
{
    public void Configure(EntityTypeBuilder<Journal> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(Journal)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasMaxLength(20);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Type).HasMaxLength(10);
        builder.Property(x => x.Description).HasMaxLength(255);

        builder.HasOne(pk => pk.DefaultDebitAccount)
               .WithMany(pk => pk.DebitJournals)
               .HasForeignKey(pk => pk.DefaultDebitAccountId)
               .OnDelete(DeleteBehavior.Restrict);
               
        builder.HasOne(j => j.DefaultCreditAccount)
               .WithMany(a => a.CreditJournals)
               .HasForeignKey(pk => pk.DefaultCreditAccountId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
