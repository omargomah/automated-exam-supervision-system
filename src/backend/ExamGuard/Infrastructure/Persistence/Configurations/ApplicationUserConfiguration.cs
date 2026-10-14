using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FName)
                .IsRequired()
                .HasMaxLength(Constants.ApplicationUser.FirstNameMaxLength);

            builder.Property(u => u.LName)
                .IsRequired()
                .HasMaxLength(Constants.ApplicationUser.LastNameMaxLength);

            builder.Ignore(u => u.FullName);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value!)
                .HasMaxLength(Constants.ApplicationUser.EmailMaxLength);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasMany(u => u.CreatedExamSessions)
                .WithOne(es => es.CreatedBy)
                .HasForeignKey(es => es.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReviewedAlertEvents)
                .WithOne(ae => ae.ReviewedBy)
                .HasForeignKey(ae => ae.ReviewedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.AuditLogs)
                .WithOne(al => al.ApplicationUser)
                .HasForeignKey(al => al.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
