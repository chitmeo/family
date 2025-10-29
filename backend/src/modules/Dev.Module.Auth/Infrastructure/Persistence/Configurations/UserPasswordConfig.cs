using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Module.Auth.Infrastructure.Persistence.Configurations;

internal class UserPasswordConfig : IEntityTypeConfiguration<UserPassword>
{
    public void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        builder.ToTable($"{Constraints.PrefixTable}{nameof(UserPassword)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x=> x.PasswordSalt).HasMaxLength(30);
        builder.Property(x=> x.Password).HasMaxLength(2000);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserPasswords)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
