using Application.IServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Application.Auth.RegisterExamAdminCommand
{
    public class RegisterExamAdminCommandHandler : IRequestHandler<RegisterExamAdminCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<RegisterExamAdminCommandHandler> _logger;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterExamAdminCommandHandler(IIdentityService identityService ,
            ILogger<RegisterExamAdminCommandHandler> logger ,
            IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _logger = logger;
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RegisterExamAdminCommand request, CancellationToken cancellationToken)
        {
            // create identity user
            Result<Guid> createAppUserResult = await _identityService.RegisterAsync(request.Email,request.Password, UserRole.ExamAdmin, cancellationToken);
            if(createAppUserResult.IsFail)
                return createAppUserResult;

            //create ExamAdmin domain entity
            Result<ApplicationUser> applcationUserResult = ApplicationUser.Create(request.FName, request.LName,request.Email,createAppUserResult.Value);
            if (applcationUserResult.IsSuccess)
            {
                await _applicationUserRepository.AddAsync(applcationUserResult.Value!);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                    return applcationUserResult;
            }

            // if ExamAdmin creation fails, delete the created identity user
            _logger.LogError(applcationUserResult.Error!.Message);
            Result deleteResult = await _identityService.DeleteAppUserAsync(createAppUserResult.Value.ToString(), cancellationToken);
            if(deleteResult.IsFail)
                _logger.LogCritical(deleteResult.Error!.Message);
            return deleteResult;
        }
    }
}
