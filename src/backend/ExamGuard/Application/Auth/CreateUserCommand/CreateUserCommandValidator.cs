using Application.IServices;
using Domain.Enums;
using Domain.Shared;
using FluentValidation;
namespace Application.Auth.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IIdentityService identityService)
        {
            RuleFor(x => x.FName)
                .NotEmpty().WithMessage("FName is required.")
                .MaximumLength(Constants.ApplicationUser.FirstNameMaxLength).WithMessage($"First name cannot exceed {Constants.ApplicationUser.FirstNameMaxLength} characters.");

            RuleFor(x => x.LName)
                .NotEmpty().WithMessage("LName is required.")
                .MaximumLength(Constants.ApplicationUser.LastNameMaxLength).WithMessage($"Last name cannot exceed {Constants.ApplicationUser.LastNameMaxLength} characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.").MustAsync(identityService.CheckEmailIsUniqueAsync);
            
            RuleFor(x => x.UserRole).IsInEnum().WithMessage($"the User Role Should be {UserRole.Proctor} or {UserRole.Supervisor}")
                .NotEqual(UserRole.ExamAdmin)
                .WithMessage("You can't create User with this role");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation is required.")
                .MinimumLength(8).WithMessage("Password confirmation must be at least 8 characters long.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }

    }
}
