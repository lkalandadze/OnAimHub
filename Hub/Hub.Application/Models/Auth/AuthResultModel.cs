#nullable disable

namespace Hub.Application.Models.Auth;

public class AuthResultModel
{
    public bool Success { get; set; }
    public string Token { get; set; }
}