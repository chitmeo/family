using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class JournalBookConfig : IEntityTypeConfiguration<JournalBook>
{
    public void Configure(EntityTypeBuilder<JournalBook> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(JournalBook)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1024).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        //Chart of Account -> Journal Book
        builder.HasOne(coa => coa.ChartOfAccount)
               .WithMany(jb => jb.JournalBooks)
               .HasForeignKey(pk => pk.ChartOfAccountId)
               .OnDelete(DeleteBehavior.Restrict);    
    }
}