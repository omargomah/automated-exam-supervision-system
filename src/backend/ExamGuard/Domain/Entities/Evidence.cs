using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Evidence : Entity
    {
        private Evidence() : base(Guid.NewGuid()) { }

        private Evidence(
            Guid id,
            Guid alertEventId,
            MediaFile media,
            FrameType frameType) : base(id)
        {
            AlertEventId = alertEventId;
            Media = media;
            FrameType = frameType;
        }

        public Guid AlertEventId { get; private set; }
        public MediaFile Media { get; private set; } = null!;
        public FrameType FrameType { get; private set; }

        public AlertEvent AlertEvent { get; private set; } = null!;

        public static Result<Evidence> Create(
            Guid alertEventId,
            MediaFile media,
            FrameType frameType,
            Guid? id = null)
        {
            if (alertEventId == Guid.Empty)
            {
                return Result.Failure<Evidence>(Error.Create("Evidence.EmptyAlertEventId", "Alert event ID cannot be empty."));
            }

            if (media is null)
            {
                return Result.Failure<Evidence>(Error.Create("Evidence.MediaNull", "Media file cannot be null."));
            }

            if (frameType == FrameType.VideoClip && media.DurationSeconds.HasValue && media.DurationSeconds.Value > Constants.Evidence.MaxVideoDurationSeconds)
            {
                return Result.Failure<Evidence>(Error.Create(
                    "Evidence.VideoDurationExceeded",
                    $"Video evidence duration cannot exceed {Constants.Evidence.MaxVideoDurationSeconds} seconds."));
            }

            return Result.Success(new Evidence(
                id ?? Guid.NewGuid(),
                alertEventId,
                media,
                frameType));
        }

        public Result UpdateMedia(MediaFile media)
        {
            if (media is null)
            {
                return Result.Failure(Error.Create("Evidence.MediaNull", "Media file cannot be null."));
            }

            Media = media;
            return Result.Success();
        }
    }
}
