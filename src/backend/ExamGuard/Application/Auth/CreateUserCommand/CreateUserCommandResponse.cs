using Domain.Enums;

namespace Application.Auth.CreateUserCommand
{
    public sealed record CreateUserCommandResponse(string Email,string FName,string LName,UserRole UserRole);
}

