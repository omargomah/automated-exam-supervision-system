using Application.Auth.CreateUserCommand;
using Application.ProctorCommandsAndQueries.Queries.GetAllProctors;
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

            TypeAdapterConfig<ApplicationUser, GetAllProctorResponse>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Name, src => $"{src.FName} {src.LName}");
        }
    }
}
