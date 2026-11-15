using Application.Auth.ConfirmEmailCommand;
using Application.Features.CreateClassroom;
using Application.IServices;
using Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.CreateClassroom
{
    public class CreateClassroomHandler : IRequestHandler<CreateClassroomCommand, Result<Guid>>
    {
        private readonly IClassRoomService _classRoomService;

        public CreateClassroomHandler(IClassRoomService classRoomService)
        {
            _classRoomService = classRoomService;
        }

        public async Task<Result<Guid>> Handle(CreateClassroomCommand command, CancellationToken cancellationToken)
        {

            return await _classRoomService.CreateClassRoomAsync(command);
        }
    }
}
