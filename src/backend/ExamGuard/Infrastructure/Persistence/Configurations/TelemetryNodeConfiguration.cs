using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TelemetryNodeConfiguration : IEntityTypeConfiguration<TelemetryNode>
    {
        public void Configure(EntityTypeBuilder<TelemetryNode> builder)
        {
            builder.ToTable("TelemetryNodes");

            builder.Property(tn => tn.MacAddress)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(Constants.TelemetryNode.MacAddressMaxLength);

            builder.Property(tn => tn.FirmwareVersion)
                .HasMaxLength(Constants.TelemetryNode.FirmwareVersionMaxLength);

            builder.HasIndex(tn => tn.MacAddress)
                .IsUnique();
        }
    }
}
