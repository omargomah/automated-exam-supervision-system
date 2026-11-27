using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.UseTpcMappingStrategy();

            builder.HasKey(d => d.Id);

            builder.Property(d => d.ClassroomId)
                .IsRequired();

            builder.Property(d => d.CurrentIpAddress)
                .IsUnicode(false)
                .HasMaxLength(Constants.Device.IpAddressMaxLength);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasConversion<string>()
                .IsUnicode(false)
                .HasMaxLength(Constants.Device.StatusMaxLength);

            builder.Property(d => d.LastSeenAt);

            builder.Property(d => d.ApiKeyHash)
                .IsRequired()
                .HasMaxLength(Constants.Device.ApiKeyHashMaxLength);

            builder.HasIndex(d => d.ClassroomId);

            builder.HasIndex(d => d.ApiKeyHash)
                .IsUnique();

            builder.HasIndex(d => new { d.Status, d.LastSeenAt });

            builder.HasOne(d => d.Classroom)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
