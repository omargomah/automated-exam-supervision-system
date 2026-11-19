namespace Application.ProctorCommandsAndQueries.Queries.GetAllProctors
{
    public sealed record GetAllProctorResponse
            (Guid Id,
            string Name,
            string Email);
}
