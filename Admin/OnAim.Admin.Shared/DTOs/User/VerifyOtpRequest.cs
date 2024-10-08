namespace OnAim.Admin.Shared.DTOs.User;

public class VerifyOtpRequest
{
    public int UserId { get; set; }
    public string Otp { get; set; }
}