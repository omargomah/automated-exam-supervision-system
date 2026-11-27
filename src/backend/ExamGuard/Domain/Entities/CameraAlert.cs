using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class CameraAlert : AlertEvent
    {
        private CameraAlert() : base() { }

        private CameraAlert(
            Guid id,
            Guid deviceId,
            Guid studentId,
            Guid examSessionId,
            VideoAlertType videoAlertType,
            decimal confidenceScore,
            MediaFile media,
            DateTime createdAt,
            AlertStatus status)
            : base(id, deviceId, examSessionId, confidenceScore, createdAt, status)
        {
            StudentId = studentId;
            VideoAlertType = videoAlertType;
            Media = media;
        }

        public Guid StudentId { get; private set; }
        public MediaFile Media { get; private set; } = null!;
        public VideoAlertType VideoAlertType { get; private set; }

        public Student Student { get; private set; } = null!;

        public static Result<CameraAlert> Create(
            Guid deviceId,
            Guid studentId,
            Guid examSessionId,
            VideoAlertType videoAlertType,
            decimal confidenceScore,
            MediaFile media,
            Guid? id = null)
        {
            var baseResult = ValidateBaseInputs(deviceId, examSessionId, confidenceScore);
            if (baseResult.IsFail)
            {
                return Result.Failure<CameraAlert>(baseResult.Error!);
            }

            if (studentId == Guid.Empty)
            {
                return Result.Failure<CameraAlert>(Error.Create("CameraAlert.EmptyStudentId", "Student ID cannot be empty."));
            }

            if (media is null)
            {
                return Result.Failure<CameraAlert>(Error.Create("CameraAlert.MediaNull", "Media file cannot be null."));
            }

            return Result.Success(new CameraAlert(
                id ?? Guid.NewGuid(),
                deviceId,
                studentId,
                examSessionId,
                videoAlertType,
                baseResult.Value,
                media,
                DateTime.UtcNow,
                AlertStatus.Pending));
        }

        public static Result<CameraAlert> Create(
            Guid deviceId,
            Guid studentId,
            Guid examSessionId,
            VideoAlertType videoAlertType,
            decimal confidenceScore,
            string mediaUrl,
            string mediaPublicId,
            Guid? id = null)
        {
            var mediaResult = MediaFile.Create(mediaUrl, mediaPublicId);
            if (mediaResult.IsFail)
            {
                return Result.Failure<CameraAlert>(mediaResult.Error!);
            }

            return Create(deviceId, studentId, examSessionId, videoAlertType, confidenceScore, mediaResult.Value!, id);
        }

        public Result UpdateMedia(MediaFile media)
        {
            if (media is null)
            {
                return Result.Failure(Error.Create("CameraAlert.MediaNull", "Media file cannot be null."));
            }

            Media = media;
            return Result.Success();
        }
    }
}
