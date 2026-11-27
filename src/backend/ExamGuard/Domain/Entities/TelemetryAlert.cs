using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities
{
    public class TelemetryAlert : AlertEvent
    {
        private TelemetryAlert() : base() { }

        private TelemetryAlert(
            Guid id,
            Guid deviceId,
            Guid examSessionId,
            TelemetryAlertType telemetryAlertType,
            decimal confidenceScore,
            decimal rfLevel,
            decimal soundLevel,
            bool isSpike,
            int? bleDeviceCount,
            int? suspiciousApCount,
            DateTime createdAt,
            AlertStatus status)
            : base(id, deviceId, examSessionId, confidenceScore, createdAt, status)
        {
            TelemetryAlertType = telemetryAlertType;
            RfLevel = rfLevel;
            SoundLevel = soundLevel;
            BleDeviceCount = bleDeviceCount;
            SuspiciousApCount = suspiciousApCount;
        }

        public decimal RfLevel { get; private set; }
        public decimal SoundLevel { get; private set; }
        public TelemetryAlertType TelemetryAlertType { get; private set; }
        public int? BleDeviceCount { get; private set; }
        public int? SuspiciousApCount { get; private set; }

        public static Result<TelemetryAlert> Create(
            Guid deviceId,
            Guid examSessionId,
            TelemetryAlertType telemetryAlertType,
            decimal confidenceScore,
            decimal rfLevel,
            decimal soundLevel,
            bool isSpike,
            int? bleDeviceCount = null,
            int? suspiciousApCount = null,
            Guid? id = null)
        {
            var baseResult = ValidateBaseInputs(deviceId, examSessionId, confidenceScore);
            if (baseResult.IsFail)
            {
                return Result.Failure<TelemetryAlert>(baseResult.Error!);
            }

            if (bleDeviceCount.HasValue && bleDeviceCount.Value < 0)
            {
                return Result.Failure<TelemetryAlert>(Error.Create(
                    "TelemetryAlert.InvalidBleDeviceCount",
                    "BLE device count cannot be negative."));
            }

            if (suspiciousApCount.HasValue && suspiciousApCount.Value < 0)
            {
                return Result.Failure<TelemetryAlert>(Error.Create(
                    "TelemetryAlert.InvalidSuspiciousApCount",
                    "Suspicious AP count cannot be negative."));
            }

            decimal roundedRfLevel = Math.Round(rfLevel, Constants.TelemetryAlert.RfLevelScale, MidpointRounding.AwayFromZero);
            decimal roundedSoundLevel = Math.Round(soundLevel, Constants.TelemetryAlert.SoundLevelScale, MidpointRounding.AwayFromZero);

            return Result.Success(new TelemetryAlert(
                id ?? Guid.NewGuid(),
                deviceId,
                examSessionId,
                telemetryAlertType,
                baseResult.Value,
                roundedRfLevel,
                roundedSoundLevel,
                isSpike,
                bleDeviceCount,
                suspiciousApCount,
                DateTime.UtcNow,
                AlertStatus.Pending));
        }
    }
}
