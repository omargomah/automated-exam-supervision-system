using Domain.Shared;
using FluentValidation;

namespace Application.Features.AddSeatsToClassroom
{
    public class AddSeatsToClassroomCommandValidator : AbstractValidator<AddSeatsToClassroomCommand>
    {
        public AddSeatsToClassroomCommandValidator()
        {
            RuleFor(c => c.ClassroomId)
                .NotEmpty()
                .WithMessage("Classroom ID is required.");

            RuleFor(c => c.Seats)
                .NotNull()
                .WithMessage("Seats list is required.")
                .NotEmpty()
                .WithMessage("At least one seat must be provided.");

            RuleForEach(c => c.Seats).ChildRules(seat =>
            {
                seat.RuleFor(s => s.Label)
                    .GreaterThan(0)
                    .WithMessage("Seat label must be a positive integer.");

                seat.RuleFor(s => s.X_Coordinate)
                    .GreaterThanOrEqualTo(Constants.Seat.MinCoordinate)
                    .WithMessage($"X coordinate must be at least {Constants.Seat.MinCoordinate}.");

                seat.RuleFor(s => s.Y_Coordinate)
                    .GreaterThanOrEqualTo(Constants.Seat.MinCoordinate)
                    .WithMessage($"Y coordinate must be at least {Constants.Seat.MinCoordinate}.");
            });
        }
    }
}
