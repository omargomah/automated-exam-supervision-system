using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class AlertEvent : Entity, IAggregateRoot
    {
        private readonly List<Evidence> _evidences = new();

        private AlertEvent() : base(Guid.NewGuid()) { }

        private AlertEvent(
            Guid id,
            Guid cameraDeviceId,
            Guid studentId,
            Guid examSessionId,
            AlertType alertType,
            decimal confidenceScore,
            DateTime createdAt,
            AlertStatus status) : base(id)
        {
            CameraDeviceId = cameraDeviceId;
            StudentId = studentId;
            ExamSessionId = examSessionId;
            AlertType = alertType;
            ConfidenceScore = confidenceScore;
            CreatedAt = createdAt;
            Status = status;
        }

        public Guid CameraDeviceId { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid ExamSessionId { get; private set; }
        public Guid? ReviewedById { get; private set; }
        public decimal ConfidenceScore { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public AlertStatus Status { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public string? ReviewerNote { get; private set; }
        public AlertType AlertType { get; private set; }

        public CameraDevice CameraDevice { get; private set; } = null!;
        public Student Student { get; private set; } = null!;
        public ExamSession ExamSession { get; private set; } = null!;
        public ApplicationUser? ReviewedBy { get; private set; }

        public IReadOnlyCollection<Evidence> Evidences => _evidences.AsReadOnly();

        public ConfidenceScore ConfidenceScoreVo => ValueObjects.ConfidenceScore.Create(ConfidenceScore).Value!;

        public static Result<AlertEvent> Create(
            Guid cameraDeviceId,
            Guid studentId,
            Guid examSessionId,
            AlertType alertType,
            decimal confidenceScore,
            Guid? id = null)
        {
            if (cameraDeviceId == Guid.Empty)
            {
                return Result.Failure<AlertEvent>(Error.Create("AlertEvent.EmptyCameraDeviceId", "Camera device ID cannot be empty."));
            }

            if (studentId == Guid.Empty)
            {
                return Result.Failure<AlertEvent>(Error.Create("AlertEvent.EmptyStudentId", "Student ID cannot be empty."));
            }

            if (examSessionId == Guid.Empty)
            {
                return Result.Failure<AlertEvent>(Error.Create("AlertEvent.EmptyExamSessionId", "Exam session ID cannot be empty."));
            }

            var scoreResult = ValueObjects.ConfidenceScore.Create(confidenceScore);
            if (scoreResult.IsFail)
            {
                return Result.Failure<AlertEvent>(scoreResult.Error!);
            }

            return Result.Success(new AlertEvent(
                id ?? Guid.NewGuid(),
                cameraDeviceId,
                studentId,
                examSessionId,
                alertType,
                scoreResult.Value!.Value,
                DateTime.UtcNow,
                AlertStatus.Pending));
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

        public void AddEvidence(Evidence evidence)
        {
            if (evidence is not null && !_evidences.Contains(evidence))
            {
                _evidences.Add(evidence);
            }
        }
    }
}
