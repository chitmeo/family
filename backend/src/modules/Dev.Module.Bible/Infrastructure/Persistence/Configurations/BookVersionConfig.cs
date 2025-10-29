using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Bible.Infrastructure.Persistence.Configurations;

internal class BookVersionConfig : IEntityTypeConfiguration<BookVersion>
{
    public void Configure(EntityTypeBuilder<BookVersion> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(BookVersion)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
    }
}
