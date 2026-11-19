using Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.CreateClassroom
{
    public sealed record CreateClassroomCommand(string name, int capacity) : IRequest<Result<Guid>>;

}
