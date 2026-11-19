using Domain.Interfaces.IRepositories;
using Domain.Shared;
using FluentValidation;

namespace Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession
{
    public class CreateExamSessionCommandValidator: AbstractValidator<CreateExamSessionCommand>
    {
        public CreateExamSessionCommandValidator(IClassroomRepository classroomRepository , IExamSessionRepository examSessionRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(Constants.ExamSession.TitleMaxLength).WithMessage($"Title must not exceed {Constants.ExamSession.TitleMaxLength} characters.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.")
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.")
                .GreaterThan(DateTime.UtcNow).WithMessage("End time must be in the future.");

            RuleFor(x => x.ClassRoomId)
                .NotEmpty().WithMessage("Classroom ID is required.")
                .MustAsync(classroomRepository.IsClassRoomExist)
                .WithMessage("Selected classroom does not exist.");
            
            RuleFor(x => x.SupervisorId)
                .NotEmpty().WithMessage("Supervisor ID is required.");

            // Validate that the selected classroom is available for the specified time slot
            RuleFor(x => x)
                .MustAsync(async (x, cancellationToken) =>
                    await examSessionRepository.IsClassRoomAvailableForSession(
                        x.StartTime,
                        x.EndTime,
                        x.ClassRoomId)).WithMessage("Selected classroom is not available for the specified time slot.");

        }

    }
}
