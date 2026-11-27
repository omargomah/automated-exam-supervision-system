using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public abstract class AlertEvent : Entity, IAggregateRoot
    {
        protected AlertEvent() : base(Guid.NewGuid()) { }

        protected AlertEvent(
            Guid id,
            Guid deviceId,
            Guid examSessionId,
            decimal confidenceScore,
            DateTime createdAt,
            AlertStatus status) : base(id)
        {
            DeviceId = deviceId;
            ExamSessionId = examSessionId;
            ConfidenceScore = confidenceScore;
            CreatedAt = createdAt;
            Status = status;
        }

        public Guid DeviceId { get; private set; }
        public Guid ExamSessionId { get; private set; }
        public Guid? ReviewedById { get; private set; }
        public decimal ConfidenceScore { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public AlertStatus Status { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public string? ReviewerNote { get; private set; }

        public ExamSession ExamSession { get; private set; } = null!;
        public ApplicationUser? ReviewedBy { get; private set; }

        public ConfidenceScore ConfidenceScoreVo => ValueObjects.ConfidenceScore.Create(ConfidenceScore).Value!;

        protected static Result<decimal> ValidateBaseInputs(
            Guid deviceId,
            Guid examSessionId,
            decimal confidenceScore)
        {
            if (deviceId == Guid.Empty)
            {
                return Result.Failure<decimal>(Error.Create("AlertEvent.EmptyDeviceId", "Device ID cannot be empty."));
            }

            if (examSessionId == Guid.Empty)
            {
                return Result.Failure<decimal>(Error.Create("AlertEvent.EmptyExamSessionId", "Exam session ID cannot be empty."));
            }

            var scoreResult = ValueObjects.ConfidenceScore.Create(confidenceScore);
            if (scoreResult.IsFail)
            {
                return Result.Failure<decimal>(scoreResult.Error!);
            }

            return Result.Success(scoreResult.Value!.Value);
        }

        public Result Review(Guid reviewerId, AlertStatus status, string? reviewerNote = null)
        {
            if (reviewerId == Guid.Empty)
            {
                return Result.Failure(Error.Create("AlertEvent.EmptyReviewerId", "Reviewer ID cannot be empty."));
            }

            if (status == AlertStatus.Pending)
            {
                return Result.Failure(Error.Create("AlertEvent.InvalidReviewStatus", "Reviewed status cannot remain Pending."));
            }

            if (!string.IsNullOrWhiteSpace(reviewerNote))
            {
                reviewerNote = reviewerNote.Trim();
                if (reviewerNote.Length > Constants.AlertEvent.ReviewerNoteMaxLength)
                {
                    return Result.Failure(Error.Create(
                        "AlertEvent.ReviewerNoteTooLong",
                        $"Reviewer note cannot exceed {Constants.AlertEvent.ReviewerNoteMaxLength} characters."));
                }
            }
            else
            {
                reviewerNote = null;
            }

            ReviewedById = reviewerId;
            Status = status;
            ReviewedAt = DateTime.UtcNow;
            ReviewerNote = reviewerNote;

            return Result.Success();
        }
    }
}
