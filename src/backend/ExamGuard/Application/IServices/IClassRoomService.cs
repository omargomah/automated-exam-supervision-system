using Application.Features.CreateClassroom;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.IServices
{
    public interface IClassRoomService
    {
        Task<Result<Guid>>CreateClassRoomAsync(CreateClassroomCommand command);

    }
}
