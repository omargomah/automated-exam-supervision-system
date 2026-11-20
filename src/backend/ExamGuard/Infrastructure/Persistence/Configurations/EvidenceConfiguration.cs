using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
    {
        public void Configure(EntityTypeBuilder<Evidence> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AlertEventId)
                .IsRequired();

            builder.Property(e => e.FrameType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.Evidence.FrameTypeMaxLength);

            builder.OwnsOne(e => e.Media, mediaBuilder =>
            {
                mediaBuilder.Property(m => m.Url)
                    .HasColumnName("MediaUrl")
                    .IsRequired()
                    .HasMaxLength(Constants.Evidence.MediaUrlMaxLength);

                mediaBuilder.Property(m => m.PublicId)
                    .HasColumnName("MediaPublicId")
                    .IsRequired()
                    .HasMaxLength(Constants.Evidence.MediaPublicIdMaxLength);

                mediaBuilder.Property(m => m.DurationSeconds)
                    .HasColumnName("DurationSeconds")
                    .IsRequired(false);
            });

            builder.HasIndex(e => e.AlertEventId);

            builder.HasOne(e => e.AlertEvent)
                .WithMany(ae => ae.Evidences)
                .HasForeignKey(e => e.AlertEventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
