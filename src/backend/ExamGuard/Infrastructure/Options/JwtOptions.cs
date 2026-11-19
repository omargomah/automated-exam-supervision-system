namespace Infrastructure.Options
{
    public class JwtOptions
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; }  = string.Empty;
        public int AccessTokenExpireAfterMinutes { get; set; }
        public int RefreshTokenExpireAfterDays { get; set; }
        public string SecretKey { get; set; } = string.Empty;
    }

}
