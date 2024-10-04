namespace OnAim.Admin.APP.Services.Abstract;

public interface IOtpService
{
    string GenerateOtp(string email); // Generates OTP for the given email
    void StoreOtp(int userId, string otp); // Stores the OTP for the user (can be in memory, cache, etc.)
    string GetStoredOtp(int userId); // Retrieves the stored OTP for validation
    bool ValidateOtp(int userId, string otp); // Validates the OTP
}
