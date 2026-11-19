using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using MediatR;

namespace Application.Features.AddSeatsToClassroom
{
    public class AddSeatsToClassroomHandler : IRequestHandler<AddSeatsToClassroomCommand, Result<List<Guid>>>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddSeatsToClassroomHandler(
            ISeatRepository seatRepository,
            IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Guid>>> Handle(AddSeatsToClassroomCommand command, CancellationToken cancellationToken)
        {
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
