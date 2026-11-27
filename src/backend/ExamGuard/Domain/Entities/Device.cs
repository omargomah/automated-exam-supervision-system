using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public abstract class Device : Entity, IAggregateRoot
    {
        protected Device() : base(Guid.NewGuid()) { }

        protected Device(
            Guid id,
            Guid classroomId,
            string apiKeyHash,
            string? currentIpAddress,
            DeviceStatus status,
            DateTime? lastSeenAt) : base(id)
        {
            ClassroomId = classroomId;
            ApiKeyHash = apiKeyHash;
            CurrentIpAddress = currentIpAddress;
            Status = status;
            LastSeenAt = lastSeenAt;
        }

        public Guid ClassroomId { get; private set; }
        public string? CurrentIpAddress { get; private set; }
        public DeviceStatus Status { get; private set; }
        public DateTime? LastSeenAt { get; private set; }
        public string ApiKeyHash { get; private set; } = string.Empty;

        public Classroom Classroom { get; private set; } = null!;

        protected static Result<(string ApiKeyHash, string? IpAddress)> ValidateBaseInputs(
            Guid classroomId,
            string apiKeyHash,
            string? currentIpAddress)
        {
            if (classroomId == Guid.Empty)
            {
                return Result.Failure<(string, string?)>(Error.Create("Device.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            if (string.IsNullOrWhiteSpace(apiKeyHash))
            {
                return Result.Failure<(string, string?)>(Error.Create("Device.EmptyApiKeyHash", "API key hash cannot be empty."));
            }

            apiKeyHash = apiKeyHash.Trim();
            if (apiKeyHash.Length > Constants.Device.ApiKeyHashMaxLength)
            {
                return Result.Failure<(string, string?)>(Error.Create(
                    "Device.ApiKeyHashTooLong",
                    $"API key hash cannot exceed {Constants.Device.ApiKeyHashMaxLength} characters."));
            }

            if (!string.IsNullOrWhiteSpace(currentIpAddress))
            {
                var ipResult = IpAddress.Create(currentIpAddress);
                if (ipResult.IsFail)
                {
                    return Result.Failure<(string, string?)>(ipResult.Error!);
                }
                currentIpAddress = ipResult.Value!.Value;
            }
            else
            {
                currentIpAddress = null;
            }

            return Result.Success((apiKeyHash, currentIpAddress));
        }

        public Result UpdateStatus(DeviceStatus status)
        {
            Status = status;
            if (status == DeviceStatus.Online)
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
            Status = DeviceStatus.Online;

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                var ipResult = IpAddress.Create(ipAddress);
                if (ipResult.IsSuccess)
                {
                    CurrentIpAddress = ipResult.Value!.Value;
                }
            }
        }

        public Result UpdateApiKeyHash(string apiKeyHash)
        {
            if (string.IsNullOrWhiteSpace(apiKeyHash))
            {
                return Result.Failure(Error.Create("Device.EmptyApiKeyHash", "API key hash cannot be empty."));
            }

            apiKeyHash = apiKeyHash.Trim();
            if (apiKeyHash.Length > Constants.Device.ApiKeyHashMaxLength)
            {
                return Result.Failure(Error.Create(
                    "Device.ApiKeyHashTooLong",
                    $"API key hash cannot exceed {Constants.Device.ApiKeyHashMaxLength} characters."));
            }

            ApiKeyHash = apiKeyHash;
            return Result.Success();
        }

        public Result Relocate(Guid newClassroomId)
        {
            if (newClassroomId == Guid.Empty)
            {
                return Result.Failure(Error.Create("Device.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            ClassroomId = newClassroomId;
            return Result.Success();
        }
    }
}
