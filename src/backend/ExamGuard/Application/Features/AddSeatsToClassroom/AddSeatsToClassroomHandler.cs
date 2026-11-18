using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using MediatR;

namespace Application.Features.AddSeatsToClassroom
{
    public class AddSeatsToClassroomHandler : IRequestHandler<AddSeatsToClassroomCommand, Result<List<Guid>>>
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddSeatsToClassroomHandler(
            IClassroomRepository classroomRepository,
            ISeatRepository seatRepository,
            IUnitOfWork unitOfWork)
        {
            _classroomRepository = classroomRepository;
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Guid>>> Handle(AddSeatsToClassroomCommand command, CancellationToken cancellationToken)
        {
            // 1. Validate classroom exists
            var classroom = await _classroomRepository.GetWithSeatsAsync(command.ClassroomId, cancellationToken);
            if (classroom is null)
            {
                return Result.Failure<List<Guid>>(
                    Error.Create("Classroom.NotFound", $"Classroom with ID '{command.ClassroomId}' was not found."));
            }

            // 2. Validate total seats won't exceed capacity
            var currentSeatCount = classroom.Seats.Count;
            var newSeatsCount = command.Seats.Count;
            if (currentSeatCount + newSeatsCount > classroom.Capacity)
            {
                return Result.Failure<List<Guid>>(
                    Error.Create("Classroom.CapacityExceeded",
                        $"Adding {newSeatsCount} seat(s) would exceed the classroom capacity of {classroom.Capacity}. " +
                        $"Current seats: {currentSeatCount}, available slots: {classroom.Capacity - currentSeatCount}."));
            }

            // 3. Validate no duplicate labels within the request itself
            var requestLabels = command.Seats.Select(s => s.Label).ToList();
            var duplicateLabels = requestLabels.GroupBy(l => l).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicateLabels.Any())
            {
                return Result.Failure<List<Guid>>(
                    Error.Create("Seat.DuplicateLabelsInRequest",
                        $"Duplicate seat labels found in the request: {string.Join(", ", duplicateLabels)}."));
            }

            // 4. Validate labels are unique within the classroom (against existing seats)
            var existingLabels = classroom.Seats.Select(s => s.Label).ToHashSet();
            var conflictingLabels = requestLabels.Where(l => existingLabels.Contains(l)).ToList();
            if (conflictingLabels.Any())
            {
                return Result.Failure<List<Guid>>(
                    Error.Create("Seat.DuplicateLabel",
                        $"Seat label(s) already exist in this classroom: {string.Join(", ", conflictingLabels)}."));
            }

            // 5. Create all seats
            var createdSeatIds = new List<Guid>();
            foreach (var seatDto in command.Seats)
            {
                var seatResult = Seat.Create(command.ClassroomId, seatDto.X_Coordinate, seatDto.Y_Coordinate, seatDto.Label);
                if (seatResult.IsFail)
                {
                    return Result.Failure<List<Guid>>(seatResult.Error!);
                }

                var newSeat = seatResult.Value!;
                await _seatRepository.AddAsync(newSeat, cancellationToken);
                createdSeatIds.Add(newSeat.Id);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Success(createdSeatIds);
        }
    }
}
