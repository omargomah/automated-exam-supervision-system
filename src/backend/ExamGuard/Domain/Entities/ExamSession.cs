using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Entities
{
    public class ExamSession : Entity, IAggregateRoot
    {
        private readonly List<ExamSeatAssignment> _examSeatAssignments = new();
        private readonly List<AlertEvent> _alertEvents = new();

        private ExamSession() : base(Guid.NewGuid()) { }

        private ExamSession(
            Guid id,
            string title,
            DateTime startTime,
            DateTime endTime,
            ExamSessionStatus status,
            Guid createdById,
            Guid classroomId) : base(id)
        {
            Title = title;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
            CreatedById = createdById;
            ClassroomId = classroomId;
        }

        public string Title { get; private set; } = string.Empty;
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ExamSessionStatus Status { get; private set; }
        public Guid CreatedById { get; private set; }
        public Guid ClassroomId { get; private set; }

        public Classroom Classroom { get; private set; } = null!;
        public ApplicationUser CreatedBy { get; private set; } = null!;

        public IReadOnlyCollection<ExamSeatAssignment> ExamSeatAssignments => _examSeatAssignments.AsReadOnly();
        public IReadOnlyCollection<AlertEvent> AlertEvents => _alertEvents.AsReadOnly();

        public static Result<ExamSession> Create(
            string title,
            DateTime startTime,
            DateTime endTime,
            Guid createdById,
            Guid classroomId,
            Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<ExamSession>(Error.Create("ExamSession.TitleEmpty", "Exam session title cannot be empty."));
            }

            title = title.Trim();

            if (title.Length > Constants.ExamSession.TitleMaxLength)
            {
                return Result.Failure<ExamSession>(Error.Create(
                    "ExamSession.TitleTooLong",
                    $"Exam session title cannot exceed {Constants.ExamSession.TitleMaxLength} characters."));
            }

            if (endTime <= startTime)
            {
                return Result.Failure<ExamSession>(Error.Create(
                    "ExamSession.InvalidSchedule",
                    "End time must be later than start time."));
            }

            if (createdById == Guid.Empty)
            {
                return Result.Failure<ExamSession>(Error.Create("ExamSession.EmptyCreatedById", "CreatedBy user ID cannot be empty."));
            }

            if (classroomId == Guid.Empty)
            {
                return Result.Failure<ExamSession>(Error.Create("ExamSession.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            return Result.Success(new ExamSession(
                id ?? Guid.NewGuid(),
                title,
                startTime,
                endTime,
                ExamSessionStatus.Scheduled,
                createdById,
                classroomId));
        }

        public Result Start()
        {
            if (Status == ExamSessionStatus.Completed)
            {
                return Result.Failure(Error.Create("ExamSession.AlreadyCompleted", "Cannot start an already completed exam session."));
            }

            Status = ExamSessionStatus.Active;
            return Result.Success();
        }

        public Result Pause()
        {
            if (Status != ExamSessionStatus.Active)
            {
                return Result.Failure(Error.Create("ExamSession.NotActive", "Only active exam sessions can be paused."));
            }

            Status = ExamSessionStatus.Paused;
            return Result.Success();
        }

        public Result Complete()
        {
            if (Status == ExamSessionStatus.Completed)
            {
                return Result.Failure(Error.Create("ExamSession.AlreadyCompleted", "Exam session is already completed."));
            }

            Status = ExamSessionStatus.Completed;
            return Result.Success();
        }

        public Result Reschedule(DateTime startTime, DateTime endTime)
        {
            if (Status == ExamSessionStatus.Completed)
            {
                return Result.Failure(Error.Create("ExamSession.AlreadyCompleted", "Cannot reschedule a completed exam session."));
            }

            if (endTime <= startTime)
            {
                return Result.Failure(Error.Create("ExamSession.InvalidSchedule", "End time must be later than start time."));
            }

            StartTime = startTime;
            EndTime = endTime;

            return Result.Success();
        }

        public Result UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure(Error.Create("ExamSession.TitleEmpty", "Exam session title cannot be empty."));
            }

            title = title.Trim();

            if (title.Length > Constants.ExamSession.TitleMaxLength)
            {
                return Result.Failure(Error.Create(
                    "ExamSession.TitleTooLong",
                    $"Exam session title cannot exceed {Constants.ExamSession.TitleMaxLength} characters."));
            }

            Title = title;
            return Result.Success();
        }
    }
}
