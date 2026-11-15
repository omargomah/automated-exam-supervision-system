using Application.Features.CreateClassroom;
using Application.IServices;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ClassRoomService : IClassRoomService
    {
        private readonly ApplicationDbContext _context;

        public ClassRoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> CreateClassRoomAsync(
            CreateClassroomCommand command)
        {
            bool exists =await _context.Classrooms.AnyAsync(c => c.Name == command.name);
            if(exists)
            {
                return Result.Failure<Guid>(
                    Error.Create(
                        "Classroom.DuplicateName",
                        "Classroom with the same name already exists."
                    ));
            }
            var classroom = Classroom.Create(command.name, command.capacity);

            if (!classroom.IsSuccess)
            {
                return Result.Failure<Guid>(classroom.Error!);
            }

            _context.Classrooms.Add(classroom.Value);


            var rowsAffected = await _context.SaveChangesAsync();

            return Result<Guid>.Success(classroom.Value.Id);
        }
    }
}