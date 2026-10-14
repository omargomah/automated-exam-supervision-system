using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Entities
{
    public class Evidence : Entity, IAggregateRoot
    {
        private Evidence() : base(Guid.NewGuid()) { }

        private Evidence(
            Guid id,
            Guid alertEventId,
            string mediaPath,
            FrameType frameType) : base(id)
        {
            AlertEventId = alertEventId;
            MediaPath = mediaPath;
            FrameType = frameType;
        }

        public Guid AlertEventId { get; private set; }
        public string MediaPath { get; private set; } = string.Empty;
        public FrameType FrameType { get; private set; }

        public AlertEvent AlertEvent { get; private set; } = null!;

        public static Result<Evidence> Create(
            Guid alertEventId,
            string mediaPath,
            FrameType frameType,
            Guid? id = null)
        {
            if (alertEventId == Guid.Empty)
            {
                return Result.Failure<Evidence>(Error.Create("Evidence.EmptyAlertEventId", "Alert event ID cannot be empty."));
            }

            if (string.IsNullOrWhiteSpace(mediaPath))
            {
                return Result.Failure<Evidence>(Error.Create("Evidence.MediaPathEmpty", "Media path cannot be empty."));
            }

            return Result.Success(new Evidence(
                id ?? Guid.NewGuid(),
                alertEventId,
                mediaPath.Trim(),
                frameType));
        }

        public Result UpdateMediaPath(string mediaPath)
        {
            if (string.IsNullOrWhiteSpace(mediaPath))
            {
                return Result.Failure(Error.Create("Evidence.MediaPathEmpty", "Media path cannot be empty."));
            }

            MediaPath = mediaPath.Trim();
            return Result.Success();
        }
    }
}
