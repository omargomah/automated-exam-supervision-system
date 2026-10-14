using Domain.Shared;
using MediatR;
namespace Application.Auth.RegisterExamAdminCommand
{
    public sealed record RegisterExamAdminCommand(string FName ,string LName,string Email, string Password, string ConfirmPassword) : IRequest<Result>;
}
