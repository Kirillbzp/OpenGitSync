namespace OpenGitSync.Server.Models
{
    public class JwtBearerTokenSettings
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiryTimeInHours { get; set; }
        public int PermanentExpiryTimeYears { get; set; }
    }
}
