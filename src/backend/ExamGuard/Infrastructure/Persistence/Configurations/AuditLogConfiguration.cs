using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(al => al.Id);

            builder.Property(al => al.Action)
                .IsRequired()
                .HasMaxLength(Constants.AuditLog.ActionMaxLength);

            builder.Property(al => al.EntityName)
                .IsRequired()
                .HasMaxLength(Constants.AuditLog.EntityNameMaxLength);

            builder.Property(al => al.EntityId);

            builder.Property(al => al.Details)
                .HasMaxLength(Constants.AuditLog.DetailsMaxLength);

            builder.Property(al => al.CreatedAt)
                .IsRequired();

            builder.Property(al => al.ApplicationUserId);

            builder.HasIndex(al => al.ApplicationUserId);

            builder.HasIndex(al => new { al.EntityName, al.EntityId });

            builder.HasIndex(al => al.CreatedAt);

            builder.HasOne(al => al.ApplicationUser)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
