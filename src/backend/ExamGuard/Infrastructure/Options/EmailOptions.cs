namespace Infrastructure.Options
{
    public class EmailOptions
    {
        public string EmailHost { get; set; }  = string.Empty;
        public int EmailPort { get; set; }
        public string EmailUsername { get; set; } = string.Empty;
        public string EmailPassword { get; set; } = string.Empty;
    }

}
