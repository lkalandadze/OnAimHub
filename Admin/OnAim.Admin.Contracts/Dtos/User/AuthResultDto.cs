namespace OnAim.Admin.Contracts.Dtos.User;

public class AuthResultDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiry { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}
