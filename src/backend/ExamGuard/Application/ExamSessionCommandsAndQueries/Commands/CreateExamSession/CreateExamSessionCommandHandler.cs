using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession
{
    public class CreateExamSessionCommandHandler : IRequestHandler<CreateExamSessionCommand, Result>
    {
        private readonly IExamSessionRepository _examSessionRepository;
        private readonly ILogger<CreateExamSessionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExamSessionCommandHandler(IExamSessionRepository examSessionRepository, ILogger<CreateExamSessionCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _examSessionRepository = examSessionRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateExamSessionCommand request,CancellationToken cancellationToken)
        {
            // create a new exam session using the provided details
            var result = ExamSession.Create(
                request.Title,
                request.StartTime,
                request.EndTime,
                request.SupervisorId,
                request.ClassRoomId);
            
            if (result.IsFail)
                return Result.Failure(result.Error!);

            // add the new exam session to the repository and save changes
            await _examSessionRepository.AddAsync(result.Value!);
            if (await _unitOfWork.SaveChangesAsync(cancellationToken) == 0)
            {
                // log an error if saving changes failed and return a failure result
                _logger.LogError("Failed to save changes.");
                return Result.Failure(Error.Create("CreateExamSessionCommandHandler", "Failed to save changes."));
            }
            return Result.Success();
        }
    }
    
}
