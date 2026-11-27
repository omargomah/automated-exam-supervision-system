using Domain.Entities;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TelemetryAlertConfiguration : IEntityTypeConfiguration<TelemetryAlert>
    {
        public void Configure(EntityTypeBuilder<TelemetryAlert> builder)
        {
            builder.ToTable("TelemetryAlerts");

            builder.Property(ta => ta.RfLevel)
                .IsRequired()
                .HasPrecision(Constants.TelemetryAlert.RfLevelPrecision, Constants.TelemetryAlert.RfLevelScale);

            builder.Property(ta => ta.SoundLevel)
                .IsRequired()
                .HasPrecision(Constants.TelemetryAlert.SoundLevelPrecision, Constants.TelemetryAlert.SoundLevelScale);


            builder.Property(ta => ta.TelemetryAlertType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.TelemetryAlert.TelemetryAlertTypeMaxLength);

            builder.Property(ta => ta.BleDeviceCount);

            builder.Property(ta => ta.SuspiciousApCount);
        }
    }
}
