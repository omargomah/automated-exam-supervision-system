using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ExamSeatAssignmentConfiguration : IEntityTypeConfiguration<ExamSeatAssignment>
    {
        public void Configure(EntityTypeBuilder<ExamSeatAssignment> builder)
        {
            builder.HasKey(esa => esa.Id);

            builder.Property(esa => esa.ExamSessionId)
                .IsRequired();

            builder.Property(esa => esa.StudentId)
                .IsRequired();

            builder.Property(esa => esa.SeatId)
                .IsRequired();

            builder.HasIndex(esa => new { esa.ExamSessionId, esa.StudentId })
                .IsUnique();

            builder.HasIndex(esa => new { esa.ExamSessionId, esa.SeatId })
                .IsUnique();

            builder.HasOne(esa => esa.ExamSession)
                .WithMany(es => es.ExamSeatAssignments)
                .HasForeignKey(esa => esa.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(esa => esa.Student)
                .WithMany(s => s.ExamSeatAssignments)
                .HasForeignKey(esa => esa.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(esa => esa.Seat)
                .WithMany(s => s.ExamSeatAssignments)
                .HasForeignKey(esa => esa.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
