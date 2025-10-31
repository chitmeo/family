
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ZstdSharp.Unsafe;

namespace Dev.Module.Accounting.Infrastructure.Persistence.Configurations;

public class AccountConfig : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {

        builder.ToTable($"{Constraints.PrefixTable}{nameof(Account)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.AccountType).HasMaxLength(10).IsRequired();

        builder.HasOne(a => a.ChartOfAccount)
               .WithMany(a => a.Accounts)
               .HasForeignKey(pk => pk.ChartOfAccountId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Parent)
               .WithMany(p => p.Children)
               .HasForeignKey(a => a.ParentId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);    
    }
}
