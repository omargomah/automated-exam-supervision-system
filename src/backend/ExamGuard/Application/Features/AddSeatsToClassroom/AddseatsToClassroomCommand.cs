using Domain.Shared;
using MediatR;

namespace Application.Features.AddSeatsToClassroom
{
    public sealed record SeatDto(int X_Coordinate, int Y_Coordinate, int Label);

    public sealed record AddSeatsToClassroomCommand(Guid ClassroomId, List<SeatDto> Seats) : IRequest<Result<List<Guid>>>;
}
