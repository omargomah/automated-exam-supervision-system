using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Entities
{
    public class ExamSeatAssignment : Entity, IAggregateRoot
    {
        private ExamSeatAssignment() : base(Guid.NewGuid()) { }

        private ExamSeatAssignment(
            Guid id,
            Guid examSessionId,
            Guid studentId,
            Guid seatId) : base(id)
        {
            ExamSessionId = examSessionId;
            StudentId = studentId;
            SeatId = seatId;
        }

        public Guid ExamSessionId { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid SeatId { get; private set; }

        public ExamSession ExamSession { get; private set; } = null!;
        public Student Student { get; private set; } = null!;
        public Seat Seat { get; private set; } = null!;

        public static Result<ExamSeatAssignment> Create(
            Guid examSessionId,
            Guid studentId,
            Guid seatId,
            Guid? id = null)
        {
            if (examSessionId == Guid.Empty)
            {
                return Result.Failure<ExamSeatAssignment>(Error.Create(
                    "ExamSeatAssignment.EmptyExamSessionId",
                    "Exam session ID cannot be empty."));
            }

            if (studentId == Guid.Empty)
            {
                return Result.Failure<ExamSeatAssignment>(Error.Create(
                    "ExamSeatAssignment.EmptyStudentId",
                    "Student ID cannot be empty."));
            }

            if (seatId == Guid.Empty)
            {
                return Result.Failure<ExamSeatAssignment>(Error.Create(
                    "ExamSeatAssignment.EmptySeatId",
                    "Seat ID cannot be empty."));
            }

            return Result.Success(new ExamSeatAssignment(
                id ?? Guid.NewGuid(),
                examSessionId,
                studentId,
                seatId));
        }

        public Result ReassignSeat(Guid newSeatId)
        {
            if (newSeatId == Guid.Empty)
            {
                return Result.Failure(Error.Create(
                    "ExamSeatAssignment.EmptySeatId",
                    "New seat ID cannot be empty."));
            }

            SeatId = newSeatId;
            return Result.Success();
        }
    }
}
