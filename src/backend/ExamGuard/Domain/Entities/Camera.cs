using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities
{
    public class Camera : Device
    {
        private Camera() : base() { }

        private Camera(
            Guid id,
            Guid classroomId,
            string apiKeyHash,
            string? currentIpAddress,
            DeviceStatus status,
            DateTime? lastSeenAt,
            string? hostName,
            string? streamUrl,
            int? gridRows,
            int? gridColumns,
            Guid? gridStartSeatId,
            DateTime? calibratedAt)
            : base(id, classroomId, apiKeyHash, currentIpAddress, status, lastSeenAt)
        {
            HostName = hostName;
            StreamUrl = streamUrl;
            GridRows = gridRows;
            GridColumns = gridColumns;
            GridStartSeatId = gridStartSeatId;
            CalibratedAt = calibratedAt;
        }

        public string? HostName { get; private set; }
        public string? StreamUrl { get; private set; }
        public int? GridRows { get; private set; }
        public int? GridColumns { get; private set; }
        public Guid? GridStartSeatId { get; private set; }
        public DateTime? CalibratedAt { get; private set; }

        public Seat? GridStartSeat { get; private set; }

        public static Result<Camera> Create(
            Guid classroomId,
            string apiKeyHash,
            string? currentIpAddress = null,
            DeviceStatus status = DeviceStatus.Offline,
            string? hostName = null,
            string? streamUrl = null,
            int? gridRows = null,
            int? gridColumns = null,
            Guid? gridStartSeatId = null,
            DateTime? calibratedAt = null,
            Guid? id = null)
        {
            var baseResult = ValidateBaseInputs(classroomId, apiKeyHash, currentIpAddress);
            if (baseResult.IsFail)
            {
                return Result.Failure<Camera>(baseResult.Error!);
            }

            if (!string.IsNullOrWhiteSpace(hostName))
            {
                hostName = hostName.Trim();
                if (hostName.Length > Constants.Camera.HostNameMaxLength)
                {
                    return Result.Failure<Camera>(Error.Create(
                        "Camera.HostNameTooLong",
                        $"Host name cannot exceed {Constants.Camera.HostNameMaxLength} characters."));
                }
            }
            else
            {
                hostName = null;
            }

            if (!string.IsNullOrWhiteSpace(streamUrl))
            {
                streamUrl = streamUrl.Trim();
                if (streamUrl.Length > Constants.Camera.StreamUrlMaxLength)
                {
                    return Result.Failure<Camera>(Error.Create(
                        "Camera.StreamUrlTooLong",
                        $"Stream URL cannot exceed {Constants.Camera.StreamUrlMaxLength} characters."));
                }
            }
            else
            {
                streamUrl = null;
            }

            if (gridRows.HasValue && gridRows.Value <= 0)
            {
                return Result.Failure<Camera>(Error.Create("Camera.InvalidGridRows", "Grid rows must be greater than zero."));
            }

            if (gridColumns.HasValue && gridColumns.Value <= 0)
            {
                return Result.Failure<Camera>(Error.Create("Camera.InvalidGridColumns", "Grid columns must be greater than zero."));
            }

            if (gridStartSeatId.HasValue && gridStartSeatId.Value == Guid.Empty)
            {
                return Result.Failure<Camera>(Error.Create("Camera.EmptyGridStartSeatId", "Grid start seat ID cannot be empty."));
            }

            return Result.Success(new Camera(
                id ?? Guid.NewGuid(),
                classroomId,
                baseResult.Value.ApiKeyHash,
                baseResult.Value.IpAddress,
                status,
                status == DeviceStatus.Online ? DateTime.UtcNow : null,
                hostName,
                streamUrl,
                gridRows,
                gridColumns,
                gridStartSeatId,
                calibratedAt));
        }

        public Result UpdateStreamInfo(string? hostName, string? streamUrl)
        {
            if (!string.IsNullOrWhiteSpace(hostName))
            {
                hostName = hostName.Trim();
                if (hostName.Length > Constants.Camera.HostNameMaxLength)
                {
                    return Result.Failure(Error.Create(
                        "Camera.HostNameTooLong",
                        $"Host name cannot exceed {Constants.Camera.HostNameMaxLength} characters."));
                }
            }
            else
            {
                hostName = null;
            }

            if (!string.IsNullOrWhiteSpace(streamUrl))
            {
                streamUrl = streamUrl.Trim();
                if (streamUrl.Length > Constants.Camera.StreamUrlMaxLength)
                {
                    return Result.Failure(Error.Create(
                        "Camera.StreamUrlTooLong",
                        $"Stream URL cannot exceed {Constants.Camera.StreamUrlMaxLength} characters."));
                }
            }
            else
            {
                streamUrl = null;
            }

            HostName = hostName;
            StreamUrl = streamUrl;
            return Result.Success();
        }

        public Result Calibrate(int gridRows, int gridColumns, Guid gridStartSeatId)
        {
            if (gridRows <= 0)
            {
                return Result.Failure(Error.Create("Camera.InvalidGridRows", "Grid rows must be greater than zero."));
            }

            if (gridColumns <= 0)
            {
                return Result.Failure(Error.Create("Camera.InvalidGridColumns", "Grid columns must be greater than zero."));
            }

            if (gridStartSeatId == Guid.Empty)
            {
                return Result.Failure(Error.Create("Camera.EmptyGridStartSeatId", "Grid start seat ID cannot be empty."));
            }

            GridRows = gridRows;
            GridColumns = gridColumns;
            GridStartSeatId = gridStartSeatId;
            CalibratedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
