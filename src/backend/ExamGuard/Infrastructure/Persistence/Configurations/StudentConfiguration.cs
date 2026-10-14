using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FName)
                .IsRequired()
                .HasMaxLength(Constants.Student.FirstNameMaxLength);

            builder.Property(s => s.LName)
                .IsRequired()
                .HasMaxLength(Constants.Student.LastNameMaxLength);

            builder.Ignore(s => s.FullName);

            builder.OwnsOne(s => s.ImageUrl, imageBuilder =>
            {
                imageBuilder.Property(img => img.Url)
                    .HasColumnName("ImageUrl")
                    .IsRequired()
                    .HasMaxLength(Constants.Student.ImageUrlMaxLength);

                imageBuilder.Property(img => img.PublicId)
                    .HasColumnName("ImagePublicId")
                    .IsRequired()
                    .HasMaxLength(Constants.Student.ImagePublicIdMaxLength);
            });

            builder.HasMany(s => s.ExamSeatAssignments)
                .WithOne(esa => esa.Student)
                .HasForeignKey(esa => esa.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.AlertEvents)
                .WithOne(ae => ae.Student)
                .HasForeignKey(ae => ae.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
