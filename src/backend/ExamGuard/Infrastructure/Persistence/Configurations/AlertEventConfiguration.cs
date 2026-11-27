using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AlertEventConfiguration : IEntityTypeConfiguration<AlertEvent>
    {
        public void Configure(EntityTypeBuilder<AlertEvent> builder)
        {
            builder.UseTpcMappingStrategy();

            builder.HasKey(ae => ae.Id);

            builder.Property(ae => ae.DeviceId)
                .IsRequired();

            builder.Property(ae => ae.ExamSessionId)
                .IsRequired();

            builder.Property(ae => ae.ReviewedById);

            builder.Property(ae => ae.ConfidenceScore)
                .IsRequired()
                .HasPrecision(Constants.AlertEvent.ConfidenceScorePrecision, Constants.AlertEvent.ConfidenceScoreScale);

            builder.Property(ae => ae.CreatedAt)
                .IsRequired();

            builder.Property(ae => ae.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.AlertEvent.StatusMaxLength);

            builder.Property(ae => ae.ReviewedAt);

            builder.Property(ae => ae.ReviewerNote)
                .HasMaxLength(Constants.AlertEvent.ReviewerNoteMaxLength);

            builder.Ignore(ae => ae.ConfidenceScoreVo);

            builder.HasIndex(ae => ae.ExamSessionId);

            builder.HasIndex(ae => new { ae.ExamSessionId, ae.CreatedAt });

            builder.HasIndex(ae => new { ae.ExamSessionId, ae.Status });

            builder.HasIndex(ae => ae.DeviceId);

            builder.HasIndex(ae => ae.ReviewedById);

            builder.HasIndex(ae => ae.Status);

            builder.HasIndex(ae => ae.CreatedAt);

            builder.HasOne(ae => ae.ExamSession)
                .WithMany(es => es.AlertEvents)
                .HasForeignKey(ae => ae.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ae => ae.ReviewedBy)
                .WithMany(u => u.ReviewedAlertEvents)
                .HasForeignKey(ae => ae.ReviewedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
