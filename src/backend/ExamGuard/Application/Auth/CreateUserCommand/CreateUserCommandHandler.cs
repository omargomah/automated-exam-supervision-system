using Application.IServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using Domain.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Auth.CreateUserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IIdentityService identityService ,
            ILogger<CreateUserCommandHandler> logger,
            IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _logger = logger;
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            
            // create identity user
            Result<Guid> createAppUserResult = await _identityService.RegisterAsync(request.Email, request.Password, request.UserRole, cancellationToken);
            if(createAppUserResult.IsFail)
                return Result.Failure<CreateUserCommandResponse>(createAppUserResult.Error!);

            //create ApplicationUser domain entity
            Result<ApplicationUser> applicationUserResult = ApplicationUser.Create(request.FName, request.LName,request.Email ,createAppUserResult.Value!);

            if (applicationUserResult.IsSuccess)
            {
                await _applicationUserRepository.AddAsync(applicationUserResult.Value!);
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return Result.Success(applicationUserResult.Value!.BuildAdapter().AddParameters("UserRole",request.UserRole).AdaptToType<CreateUserCommandResponse>());
                }
                else
                {
                    // if application user creation fails, delete the created identity user
                    _logger.LogError("Failed to save changes.");
                    Result deleteResult = await _identityService.DeleteAppUserAsync(createAppUserResult.Value!.ToString(), cancellationToken);
                    if (deleteResult.IsFail)
                        _logger.LogCritical(deleteResult.Error!.Message);
                    return Result.Failure<CreateUserCommandResponse>(Error.Create("CreateUserCommandHandler", "Failed to save changes."));
                }
            }
            else
            {
                // if application user creation fails, delete the created identity user
                _logger.LogError(applicationUserResult.Error!.Message);
                Result deleteResult = await _identityService.DeleteAppUserAsync(createAppUserResult.Value!.ToString(), cancellationToken);
                if(deleteResult.IsFail)
                    _logger.LogCritical(deleteResult.Error!.Message);
                return Result.Failure<CreateUserCommandResponse>(deleteResult.Error!);
            }

        }
    }
}
