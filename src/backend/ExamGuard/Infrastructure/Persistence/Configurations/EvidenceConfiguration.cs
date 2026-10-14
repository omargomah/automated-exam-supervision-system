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

            builder.Property(e => e.MediaPath)
                .IsRequired()
                .HasMaxLength(Constants.Evidence.MediaPathMaxLength);

            builder.Property(e => e.FrameType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.Evidence.FrameTypeMaxLength);

            builder.HasIndex(e => e.AlertEventId);

            builder.HasOne(e => e.AlertEvent)
                .WithMany(ae => ae.Evidences)
                .HasForeignKey(e => e.AlertEventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
