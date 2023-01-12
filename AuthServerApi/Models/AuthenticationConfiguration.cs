namespace AuthServerApi.Models
{
    public class AuthenticationConfiguration
    {
        public string AccesTokenSecret { get; set; }
        public int AccesTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
