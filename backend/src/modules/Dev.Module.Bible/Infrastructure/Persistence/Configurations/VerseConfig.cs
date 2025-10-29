using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Bible.Infrastructure.Persistence.Configurations;

internal class VerseConfig : IEntityTypeConfiguration<Verse>
{
    public void Configure(EntityTypeBuilder<Verse> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(Verse)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.VerseText).IsRequired();
    }
}
