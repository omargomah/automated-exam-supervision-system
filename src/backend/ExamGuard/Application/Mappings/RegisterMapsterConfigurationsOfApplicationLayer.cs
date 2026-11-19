using Application.Auth.CreateUserCommand;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Mappings
{
    public static class RegisterMapsterConfigurationsOfApplicationLayer
    {
        public static void AddApplicationMappings(this IServiceCollection services)
        {
            TypeAdapterConfig<ApplicationUser, CreateUserCommandResponse>
            .NewConfig()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FName, src => src.FName)
            .Map(dest => dest.LName, src => src.LName)
            .Map(dest => dest.UserRole, "UserRole");

        }
    }
}
