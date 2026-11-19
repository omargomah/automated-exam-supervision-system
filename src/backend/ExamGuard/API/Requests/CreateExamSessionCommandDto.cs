namespace API.Requests
{
    public sealed record CreateExamSessionCommandDto(string Title, DateTime StartTime, DateTime EndTime, Guid ClassRoomId);
}
