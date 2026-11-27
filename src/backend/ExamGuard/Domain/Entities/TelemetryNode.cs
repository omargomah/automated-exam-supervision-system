using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities
{
    public class TelemetryNode : Device
    {
        private TelemetryNode() : base() { }

        private TelemetryNode(
            Guid id,
            Guid classroomId,
            string apiKeyHash,
            string macAddress,
            string? currentIpAddress,
            DeviceStatus status,
            DateTime? lastSeenAt,
            string? firmwareVersion)
            : base(id, classroomId, apiKeyHash, currentIpAddress, status, lastSeenAt)
        {
            MacAddress = macAddress;
            FirmwareVersion = firmwareVersion;
        }

        public string MacAddress { get; private set; } = string.Empty;
        public string? FirmwareVersion { get; private set; }

        public static Result<TelemetryNode> Create(
            Guid classroomId,
            string macAddress,
            string apiKeyHash,
            string? currentIpAddress = null,
            DeviceStatus status = DeviceStatus.Offline,
            string? firmwareVersion = null,
            Guid? id = null)
        {
            var baseResult = ValidateBaseInputs(classroomId, apiKeyHash, currentIpAddress);
            if (baseResult.IsFail)
            {
                return Result.Failure<TelemetryNode>(baseResult.Error!);
            }

            var macResult = ValueObjects.MacAddress.Create(macAddress);
            if (macResult.IsFail)
            {
                return Result.Failure<TelemetryNode>(macResult.Error!);
            }

            if (!string.IsNullOrWhiteSpace(firmwareVersion))
            {
                firmwareVersion = firmwareVersion.Trim();
                if (firmwareVersion.Length > Constants.TelemetryNode.FirmwareVersionMaxLength)
                {
                    return Result.Failure<TelemetryNode>(Error.Create(
                        "TelemetryNode.FirmwareVersionTooLong",
                        $"Firmware version cannot exceed {Constants.TelemetryNode.FirmwareVersionMaxLength} characters."));
                }
            }
            else
            {
                firmwareVersion = null;
            }

            return Result.Success(new TelemetryNode(
                id ?? Guid.NewGuid(),
                classroomId,
                baseResult.Value.ApiKeyHash,
                macResult.Value!.Value,
                baseResult.Value.IpAddress,
                status,
                status == DeviceStatus.Online ? DateTime.UtcNow : null,
                firmwareVersion));
        }

        public Result UpdateFirmwareVersion(string? firmwareVersion)
        {
            if (!string.IsNullOrWhiteSpace(firmwareVersion))
            {
                firmwareVersion = firmwareVersion.Trim();
                if (firmwareVersion.Length > Constants.TelemetryNode.FirmwareVersionMaxLength)
                {
                    return Result.Failure(Error.Create(
                        "TelemetryNode.FirmwareVersionTooLong",
                        $"Firmware version cannot exceed {Constants.TelemetryNode.FirmwareVersionMaxLength} characters."));
                }
                FirmwareVersion = firmwareVersion;
            }
            else
            {
                FirmwareVersion = null;
            }

            return Result.Success();
        }
    }
}
