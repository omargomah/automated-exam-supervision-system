using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ClassroomId)
                .IsRequired();

            builder.Property(s => s.X_Coordinate)
                .IsRequired();

            builder.Property(s => s.Y_Coordinate)
                .IsRequired();

            builder.Property(s => s.Label)
                .IsRequired();

            builder.Ignore(s => s.Coordinates);

            builder.HasIndex(s => new { s.ClassroomId, s.Label })
                .IsUnique();

            builder.HasIndex(s => new { s.ClassroomId, s.X_Coordinate, s.Y_Coordinate })
                .IsUnique();

            builder.HasOne(s => s.Classroom)
                .WithMany(c => c.Seats)
                .HasForeignKey(s => s.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.ExamSeatAssignments)
                .WithOne(esa => esa.Seat)
                .HasForeignKey(esa => esa.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
