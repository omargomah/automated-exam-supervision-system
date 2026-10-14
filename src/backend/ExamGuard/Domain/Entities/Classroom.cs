using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Entities
{
    public class Classroom : Entity, IAggregateRoot
    {
        private readonly List<Seat> _seats = new();
        private readonly List<CameraDevice> _cameraDevices = new();
        private readonly List<ExamSession> _examSessions = new();

        private Classroom() : base(Guid.NewGuid()) { }

        private Classroom(Guid id, string name, int capacity) : base(id)
        {
            Name = name;
            Capacity = capacity;
        }

        public string Name { get; private set; } = string.Empty;
        public int Capacity { get; private set; }

        public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
        public IReadOnlyCollection<CameraDevice> CameraDevices => _cameraDevices.AsReadOnly();
        public IReadOnlyCollection<ExamSession> ExamSessions => _examSessions.AsReadOnly();

        public static Result<Classroom> Create(string name, int capacity, Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Classroom>(Error.Create("Classroom.NameEmpty", "Classroom name cannot be empty."));
            }

            name = name.Trim();

            if (name.Length > Constants.Classroom.NameMaxLength)
            {
                return Result.Failure<Classroom>(Error.Create(
                    "Classroom.NameTooLong",
                    $"Classroom name cannot exceed {Constants.Classroom.NameMaxLength} characters."));
            }

            if (capacity < Constants.Classroom.MinCapacity)
            {
                return Result.Failure<Classroom>(Error.Create(
                    "Classroom.InvalidCapacity",
                    $"Classroom capacity must be at least {Constants.Classroom.MinCapacity}."));
            }

            return Result.Success(new Classroom(id ?? Guid.NewGuid(), name, capacity));
        }

        public Result Update(string name, int capacity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure(Error.Create("Classroom.NameEmpty", "Classroom name cannot be empty."));
            }

            name = name.Trim();

            if (name.Length > Constants.Classroom.NameMaxLength)
            {
                return Result.Failure(Error.Create(
                    "Classroom.NameTooLong",
                    $"Classroom name cannot exceed {Constants.Classroom.NameMaxLength} characters."));
            }

            if (capacity < Constants.Classroom.MinCapacity)
            {
                return Result.Failure(Error.Create(
                    "Classroom.InvalidCapacity",
                    $"Classroom capacity must be at least {Constants.Classroom.MinCapacity}."));
            }

            Name = name;
            Capacity = capacity;

            return Result.Success();
        }
    }
}
