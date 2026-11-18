using API.Requests;
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
                .Map(dest => dest.title, src => src.title.Trim())
                .Map(dest => dest.SupervisorId, "SupervisorId");
        }
    }
}
