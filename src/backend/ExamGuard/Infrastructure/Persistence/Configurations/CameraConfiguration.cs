using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CameraConfiguration : IEntityTypeConfiguration<Camera>
    {
        public void Configure(EntityTypeBuilder<Camera> builder)
        {
            builder.ToTable("Cameras");

            builder.Property(c => c.HostName)
                .HasMaxLength(Constants.Camera.HostNameMaxLength);

            builder.Property(c => c.StreamUrl)
                .HasMaxLength(Constants.Camera.StreamUrlMaxLength);

            builder.Property(c => c.GridRows);

            builder.Property(c => c.GridColumns);

            builder.Property(c => c.GridStartSeatId);

            builder.Property(c => c.CalibratedAt);

            builder.HasIndex(c => c.GridStartSeatId);

            builder.HasOne(c => c.GridStartSeat)
                .WithMany()
                .HasForeignKey(c => c.GridStartSeatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
