using API.Requests;
using Application.Auth.CreateUserCommand;
using Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession;
using Mapster;

namespace API.Mappings
{
    public static class RegisterMapsterConfigurationsOfAPILayer
    {
        public static void AddApiMappings(this IServiceCollection services)
        {
            TypeAdapterConfig<CreateExamSessionCommandDto, CreateExamSessionCommand>
                .NewConfig()
                .Map(dest => dest.Title, src => src.Title.Trim())
                .Map(dest => dest.SupervisorId, "SupervisorId");

            TypeAdapterConfig<CreateProctorDto, CreateUserCommand>
                .NewConfig()
                .Map(dest => dest.Email, src => src.Email.Trim())
                .Map(dest => dest.FName, src => src.FName.Trim())
                .Map(dest => dest.LName, src => src.LName.Trim())
                .Map(dest => dest.UserRole,"UserRole")
                .Map(dest => dest.Password, src => src.Password)
                .Map(dest => dest.ConfirmPassword, src => src.ConfirmPassword);
        }
    }
}
