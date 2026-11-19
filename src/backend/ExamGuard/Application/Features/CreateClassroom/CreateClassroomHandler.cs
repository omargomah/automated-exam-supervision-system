using Application.Auth.ConfirmEmailCommand;
using Application.IServices;
using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using MediatR;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.CreateClassroom
{
    public class CreateClassroomHandler : IRequestHandler<CreateClassroomCommand, Result<Guid>>
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClassroomHandler(IClassroomRepository classroomRepository, IUnitOfWork unitOfWork)
        {
            _classroomRepository = classroomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateClassroomCommand command, CancellationToken cancellationToken)
        {

            var classroom = Classroom.Create(command.name, command.capacity);
            if (!classroom.IsSuccess)
                return Result.Failure<Guid>(classroom.Error!);

            var newClassroom = classroom.Value!;
            await _classroomRepository.AddAsync(newClassroom, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(newClassroom.Id);
        }
    }
}
