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
            builder.HasKey(ae => ae.Id);

            builder.Property(ae => ae.CameraDeviceId)
                .IsRequired();

            builder.Property(ae => ae.StudentId)
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

            builder.Property(ae => ae.AlertType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.AlertEvent.AlertTypeMaxLength);

            builder.Ignore(ae => ae.ConfidenceScoreVo);

            builder.HasIndex(ae => ae.ExamSessionId);

            builder.HasIndex(ae => ae.StudentId);

            builder.HasIndex(ae => ae.CameraDeviceId);

            builder.HasIndex(ae => ae.ReviewedById);

            builder.HasIndex(ae => ae.Status);

            builder.HasIndex(ae => ae.CreatedAt);

            builder.HasOne(ae => ae.CameraDevice)
                .WithMany(cd => cd.AlertEvents)
                .HasForeignKey(ae => ae.CameraDeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ae => ae.Student)
                .WithMany(s => s.AlertEvents)
                .HasForeignKey(ae => ae.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ae => ae.ExamSession)
                .WithMany(es => es.AlertEvents)
                .HasForeignKey(ae => ae.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ae => ae.ReviewedBy)
                .WithMany(u => u.ReviewedAlertEvents)
                .HasForeignKey(ae => ae.ReviewedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ae => ae.Evidences)
                .WithOne(e => e.AlertEvent)
                .HasForeignKey(e => e.AlertEventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
