using Domain.Interfaces.IRepositories;
using Domain.Shared;
using FluentValidation;

namespace Application.Features.CreateClassroom
{
    public class CreateClassroomCommandValidator : AbstractValidator<CreateClassroomCommand>
    {
        public CreateClassroomCommandValidator(IClassroomRepository classroomRepository)
        {
            RuleFor(c => c.name)
                .NotEmpty()
                .MaximumLength(Constants.Classroom.NameMaxLength)
                .MustAsync((name, cancellationToken) =>
            classroomRepository.IsNameUniqueAsync(name, null, cancellationToken))
                .WithMessage("Classroom with the same name already exists.");

            RuleFor(c => c.capacity)
                .GreaterThanOrEqualTo(Constants.Classroom.MinCapacity);
        }
    }
}
