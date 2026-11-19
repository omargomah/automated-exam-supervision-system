using Domain.Interfaces.IRepositories;
using Domain.Shared;
using FluentValidation;

namespace Application.Features.AddSeatsToClassroom
{
    public class AddSeatsToClassroomCommandValidator : AbstractValidator<AddSeatsToClassroomCommand>
    {
        public AddSeatsToClassroomCommandValidator(IClassroomRepository classroomRepository)
        {
            RuleFor(c => c.ClassroomId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Classroom ID is required.");

            RuleFor(c => c.Seats)
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

            RuleFor(c => c.Seats)
                .Must(seats => seats == null || seats.Select(s => s.Label).Distinct().Count() == seats.Count)
                .WithMessage("Duplicate seat labels found in the request.");

            RuleFor(c => c)
                .CustomAsync(async (command, context, cancellationToken) =>
                {
                    if (command.ClassroomId == Guid.Empty || command.Seats == null || command.Seats.Count == 0)
                        return;

                    var classroom = await classroomRepository.GetWithSeatsAsync(command.ClassroomId, cancellationToken);
                    if (classroom is null)
                    {
                        context.AddFailure(nameof(command.ClassroomId), "Classroom does not exist.");
                        return;
                    }

                    if (classroom.Seats.Count + command.Seats.Count > classroom.Capacity)
                    {
                        context.AddFailure("Seats",
                            $"Adding {command.Seats.Count} seat(s) exceeds classroom capacity of {classroom.Capacity}. " +
                            $"Current seats: {classroom.Seats.Count}, available slots: {classroom.Capacity - classroom.Seats.Count}.");
                    }

                    var existingLabels = classroom.Seats.Select(s => s.Label).ToHashSet();
                    var conflictingLabels = command.Seats
                        .Where(s => existingLabels.Contains(s.Label))
                        .Select(s => s.Label)
                        .Distinct()
                        .ToList();

                    if (conflictingLabels.Count != 0)
                    {
                        context.AddFailure("Seats",
                            $"Seat label(s) already exist in this classroom: {string.Join(", ", conflictingLabels)}.");
                    }
                });
        }
    }
}
