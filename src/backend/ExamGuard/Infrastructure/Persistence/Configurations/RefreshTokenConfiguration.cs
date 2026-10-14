using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(rt => rt.ExpiresOn)
                .IsRequired();

            builder.Property(rt => rt.CreatedOn)
                .IsRequired();

            builder.Property(rt => rt.RevokedOn);

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Ignore(rt => rt.IsExpired);

            builder.Ignore(rt => rt.IsActive);

            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.HasIndex(rt => rt.UserId);

            builder.HasOne<AppUser>()
                .WithMany(u => u.refreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
