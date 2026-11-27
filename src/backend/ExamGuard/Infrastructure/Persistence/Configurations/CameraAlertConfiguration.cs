using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CameraAlertConfiguration : IEntityTypeConfiguration<CameraAlert>
    {
        public void Configure(EntityTypeBuilder<CameraAlert> builder)
        {
            builder.ToTable("CameraAlerts");

            builder.Property(ca => ca.StudentId)
                .IsRequired();

            builder.Property(ca => ca.VideoAlertType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.CameraAlert.VideoAlertTypeMaxLength);

            builder.OwnsOne(ca => ca.Media, mediaBuilder =>
            {
                mediaBuilder.Property(m => m.Url)
                    .HasColumnName("Url")
                    .IsRequired()
                    .HasMaxLength(Constants.MediaFile.UrlMaxLength);

                mediaBuilder.Property(m => m.PublicId)
                    .HasColumnName("PublicId")
                    .IsRequired()
                    .HasMaxLength(Constants.MediaFile.PublicIdMaxLength);
            });

            builder.HasIndex(ca => ca.StudentId);

            builder.HasOne(ca => ca.Student)
                .WithMany(s => s.CameraAlerts)
                .HasForeignKey(ca => ca.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
