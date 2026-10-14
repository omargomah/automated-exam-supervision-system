using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CameraDeviceConfiguration : IEntityTypeConfiguration<CameraDevice>
    {
        public void Configure(EntityTypeBuilder<CameraDevice> builder)
        {
            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.ClassroomId)
                .IsRequired();

            builder.Property(cd => cd.MacAddress)
                .IsRequired()
                .HasMaxLength(Constants.CameraDevice.MacAddressMaxLength);

            builder.Property(cd => cd.CurrentIpAddress)
                .HasMaxLength(Constants.CameraDevice.IpAddressMaxLength);

            builder.Property(cd => cd.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.CameraDevice.StatusMaxLength);

            builder.Property(cd => cd.LastSeenAt);

            builder.HasIndex(cd => cd.MacAddress)
                .IsUnique();

            builder.HasIndex(cd => cd.ClassroomId);

            builder.HasOne(cd => cd.Classroom)
                .WithMany(c => c.CameraDevices)
                .HasForeignKey(cd => cd.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cd => cd.AlertEvents)
                .WithOne(ae => ae.CameraDevice)
                .HasForeignKey(ae => ae.CameraDeviceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
