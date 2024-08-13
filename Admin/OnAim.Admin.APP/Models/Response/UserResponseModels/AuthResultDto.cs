namespace OnAim.Admin.APP.Models.Response.User
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public int StatusCode { get; set; }
    }
}
