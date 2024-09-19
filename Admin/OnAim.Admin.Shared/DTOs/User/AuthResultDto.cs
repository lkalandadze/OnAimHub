namespace OnAim.Admin.Shared.DTOs.User
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
        public int StatusCode { get; set; }
    }
}
