namespace OnAim.Admin.Contracts.Dtos.User;

public class VerifyOtpRequest
{
    public int UserId { get; set; }
    public string Otp { get; set; }
}