using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ExamSessionConfiguration : IEntityTypeConfiguration<ExamSession>
    {
        public void Configure(EntityTypeBuilder<ExamSession> builder)
        {
            builder.HasKey(es => es.Id);

            builder.Property(es => es.Title)
                .IsRequired()
                .HasMaxLength(Constants.ExamSession.TitleMaxLength);

            builder.Property(es => es.StartTime)
                .IsRequired();

            builder.Property(es => es.EndTime)
                .IsRequired();

            builder.Property(es => es.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.ExamSession.StatusMaxLength);

            builder.Property(es => es.CreatedById)
                .IsRequired();

            builder.Property(es => es.ClassroomId)
                .IsRequired();

            builder.HasIndex(es => es.ClassroomId);

            builder.HasIndex(es => es.CreatedById);

            builder.HasIndex(es => es.Status);

            builder.HasIndex(es => new { es.StartTime, es.EndTime });

            builder.HasOne(es => es.Classroom)
                .WithMany(c => c.ExamSessions)
                .HasForeignKey(es => es.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(es => es.CreatedBy)
                .WithMany(u => u.CreatedExamSessions)
                .HasForeignKey(es => es.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(es => es.ExamSeatAssignments)
                .WithOne(esa => esa.ExamSession)
                .HasForeignKey(esa => esa.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(es => es.AlertEvents)
                .WithOne(ae => ae.ExamSession)
                .HasForeignKey(ae => ae.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
