using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class CameraDevice : Entity, IAggregateRoot
    {
        private readonly List<AlertEvent> _alertEvents = new();

        private CameraDevice() : base(Guid.NewGuid()) { }

        private CameraDevice(
            Guid id,
            Guid classroomId,
            string macAddress,
            string? currentIpAddress,
            CameraStatus status,
            DateTime? lastSeenAt) : base(id)
        {
            ClassroomId = classroomId;
            MacAddress = macAddress;
            CurrentIpAddress = currentIpAddress;
            Status = status;
            LastSeenAt = lastSeenAt;
        }

        public Guid ClassroomId { get; private set; }
        public string MacAddress { get; private set; } = string.Empty;
        public string? CurrentIpAddress { get; private set; }
        public CameraStatus Status { get; private set; }
        public DateTime? LastSeenAt { get; private set; }

        public Classroom Classroom { get; private set; } = null!;
        public IReadOnlyCollection<AlertEvent> AlertEvents => _alertEvents.AsReadOnly();

        public static Result<CameraDevice> Create(
            Guid classroomId,
            string macAddress,
            string? currentIpAddress = null,
            CameraStatus status = CameraStatus.Offline,
            Guid? id = null)
        {
            if (classroomId == Guid.Empty)
            {
                return Result.Failure<CameraDevice>(Error.Create("CameraDevice.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            var macResult = ValueObjects.MacAddress.Create(macAddress);
            if (macResult.IsFail)
            {
                return Result.Failure<CameraDevice>(macResult.Error!);
            }

            if (!string.IsNullOrWhiteSpace(currentIpAddress))
            {
                var ipResult = IpAddress.Create(currentIpAddress);
                if (ipResult.IsFail)
                {
                    return Result.Failure<CameraDevice>(ipResult.Error!);
                }
                currentIpAddress = ipResult.Value!.Value;
            }
            else
            {
                currentIpAddress = null;
            }

            return Result.Success(new CameraDevice(
                id ?? Guid.NewGuid(),
                classroomId,
                macResult.Value!.Value,
                currentIpAddress,
                status,
                status == CameraStatus.Online ? DateTime.UtcNow : null));
        }

        public Result UpdateStatus(CameraStatus status)
        {
            Status = status;
            if (status == CameraStatus.Online)
            {
                LastSeenAt = DateTime.UtcNow;
            }

            return Result.Success();
        }

        public Result UpdateIpAddress(string? ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                var ipResult = IpAddress.Create(ipAddress);
                if (ipResult.IsFail)
                {
                    return Result.Failure(ipResult.Error!);
                }
                CurrentIpAddress = ipResult.Value!.Value;
            }
            else
            {
                CurrentIpAddress = null;
            }

            return Result.Success();
        }

        public void RecordHeartbeat(string? ipAddress = null)
        {
            LastSeenAt = DateTime.UtcNow;
            Status = CameraStatus.Online;

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                var ipResult = IpAddress.Create(ipAddress);
                if (ipResult.IsSuccess)
                {
                    CurrentIpAddress = ipResult.Value!.Value;
                }
            }
        }

        public Result Relocate(Guid newClassroomId)
        {
            if (newClassroomId == Guid.Empty)
            {
                return Result.Failure(Error.Create("CameraDevice.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            ClassroomId = newClassroomId;
            return Result.Success();
        }
    }
}
