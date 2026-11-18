namespace API.Requests
{
    public sealed record CreateExamSessionCommandDto(string title, DateTime startTime, DateTime endTime, Guid ClassRoomId);
}
