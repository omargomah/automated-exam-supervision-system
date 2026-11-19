using Application.Auth.CreateUserCommand;
using Domain.Enums;
using Domain.Shared;
using MediatR;

namespace API.Requests
{
    public sealed record CreateProctorDto(string Email, string FName, string LName, string Password, string ConfirmPassword);

}
