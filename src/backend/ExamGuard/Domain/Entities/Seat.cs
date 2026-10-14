using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Seat : Entity, IAggregateRoot
    {
        private readonly List<ExamSeatAssignment> _examSeatAssignments = new();

        private Seat() : base(Guid.NewGuid()) { }

        private Seat(Guid id, Guid classroomId, int xCoordinate, int yCoordinate, int label) : base(id)
        {
            ClassroomId = classroomId;
            X_Coordinate = xCoordinate;
            Y_Coordinate = yCoordinate;
            Label = label;
        }

        public Guid ClassroomId { get; private set; }
        public int X_Coordinate { get; private set; }
        public int Y_Coordinate { get; private set; }
        public int Label { get; private set; }

        public Classroom Classroom { get; private set; } = null!;
        public IReadOnlyCollection<ExamSeatAssignment> ExamSeatAssignments => _examSeatAssignments.AsReadOnly();

        public Coordinates Coordinates => Coordinates.Create(X_Coordinate, Y_Coordinate).Value;

        public static Result<Seat> Create(Guid classroomId, int xCoordinate, int yCoordinate, int label, Guid? id = null)
        {
            if (classroomId == Guid.Empty)
            {
                return Result.Failure<Seat>(Error.Create("Seat.EmptyClassroomId", "Classroom ID cannot be empty."));
            }

            var coordinatesResult = Coordinates.Create(xCoordinate, yCoordinate);
            if (coordinatesResult.IsFail)
            {
                return Result.Failure<Seat>(coordinatesResult.Error!);
            }

            if (label <= 0)
            {
                return Result.Failure<Seat>(Error.Create("Seat.InvalidLabel", "Seat label must be a positive integer."));
            }

            return Result.Success(new Seat(id ?? Guid.NewGuid(), classroomId, xCoordinate, yCoordinate, label));
        }

        public Result UpdatePosition(int xCoordinate, int yCoordinate)
        {
            var coordinatesResult = Coordinates.Create(xCoordinate, yCoordinate);
            if (coordinatesResult.IsFail)
            {
                return Result.Failure(coordinatesResult.Error!);
            }

            X_Coordinate = xCoordinate;
            Y_Coordinate = yCoordinate;

            return Result.Success();
        }

        public Result UpdateLabel(int label)
        {
            if (label <= 0)
            {
                return Result.Failure(Error.Create("Seat.InvalidLabel", "Seat label must be a positive integer."));
            }

            Label = label;

            return Result.Success();
        }
    }
}
