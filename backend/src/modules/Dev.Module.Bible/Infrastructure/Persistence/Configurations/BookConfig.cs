using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Bible.Infrastructure.Persistence.Configurations;

internal class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(Book)}");
        builder.HasKey(x=> x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x=> x.Abbreviation).HasMaxLength(50).IsRequired();        
    }
}
