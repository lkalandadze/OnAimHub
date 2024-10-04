using OnAim.Admin.APP.Services.Abstract;
using System.Collections.Concurrent;

namespace OnAim.Admin.APP.Services.AuthServices;

public class OtpService : IOtpService
{
    private static readonly ConcurrentDictionary<int, (string Otp, DateTime Expiration)> _otpStore =
        new ConcurrentDictionary<int, (string Otp, DateTime Expiration)>();

    private readonly int _otpLength = 6;
    private readonly int _otpExpiryMinutes = 10;

    public string GenerateOtp(string email)
    {
        var random = new Random();
        var otp = new string(Enumerable.Range(0, _otpLength).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());

        return otp;
    }

    public void StoreOtp(int userId, string otp)
    {
        var expirationTime = DateTime.UtcNow.AddMinutes(_otpExpiryMinutes);
        _otpStore[userId] = (otp, expirationTime);
    }

    public string GetStoredOtp(int userId)
    {
        if (_otpStore.TryGetValue(userId, out var otpEntry) && otpEntry.Expiration > DateTime.UtcNow)
        {
            return otpEntry.Otp;
        }

        return null;
    }

    public bool ValidateOtp(int userId, string otp)
    {
        var storedOtp = GetStoredOtp(userId);
        return storedOtp != null && storedOtp == otp;
    }
}
