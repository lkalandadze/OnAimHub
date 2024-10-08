namespace OnAim.Admin.APP.Services.Abstract;

public interface IOtpService
{
    string GenerateOtp(string email);
    void StoreOtp(int userId, string otp);
    Task<string> GetStoredOtp(int userId);
    Task<bool> ValidateOtp(int userId, string otp);
}
