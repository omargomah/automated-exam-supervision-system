namespace API.Requests
{
    public sealed record CreateUserDto(string Email, string FName, string LName, string Password, string ConfirmPassword);

}
