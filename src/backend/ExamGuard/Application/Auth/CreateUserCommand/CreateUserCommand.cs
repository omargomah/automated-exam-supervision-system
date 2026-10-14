using Domain.Enums;
using Domain.Shared;
using MediatR;

namespace Application.Auth.CreateUserCommand
{
    public sealed record CreateUserCommand(string Email , string FName , string LName, UserRole UserRole,string Password, string ConfirmPassword) : IRequest<Result<CreateUserCommandResponse>>;
}

