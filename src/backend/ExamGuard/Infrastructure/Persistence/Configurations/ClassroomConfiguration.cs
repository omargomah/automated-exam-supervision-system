using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(Constants.Classroom.NameMaxLength);

            builder.Property(c => c.Capacity)
                .IsRequired();

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(c => c.Seats)
                .WithOne(s => s.Classroom)
                .HasForeignKey(s => s.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CameraDevices)
                .WithOne(cd => cd.Classroom)
                .HasForeignKey(cd => cd.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.ExamSessions)
                .WithOne(es => es.Classroom)
                .HasForeignKey(es => es.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
