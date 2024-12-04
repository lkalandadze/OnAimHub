using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.APP.Services.AdminServices.EmailServices;

namespace OnAim.Admin.APP.Services.Admin.AuthServices;

public class OtpService : IOtpService
{
    private readonly int _otpLength = 6;
    private readonly int _otpExpiryMinutes = 10;
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _repository;

    public OtpService(IRepository<OnAim.Admin.Domain.Entities.User> repository)
    {
        _repository = repository;
    }

    public string GenerateOtp(string email)
    {
        var random = new Random();
        var otp = new string(Enumerable.Range(0, _otpLength).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());

        return otp;
    }

    public async void StoreOtp(int userId, string otp)
    {
        var expirationTime = DateTime.UtcNow.AddMinutes(_otpExpiryMinutes);
        var user = await _repository.Query(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync();
        if (user != null)
        {
            user.VerificationCode = otp;
            user.VerificationPurpose = VerificationPurpose.Login;
            user.VerificationCodeExpiration = expirationTime;
            await _repository.CommitChanges();
        }
    }

    public async Task<string> GetStoredOtp(int userId)
    {
        var userOtp = await _repository.Query(uo =>
                uo.Id == userId &&
                uo.VerificationPurpose == VerificationPurpose.Login &&
                uo.VerificationCodeExpiration > DateTime.UtcNow)
                .FirstOrDefaultAsync();

        return userOtp?.VerificationCode;
    }

    public async Task<bool> ValidateOtp(int userId, string otp)
    {
        var storedOtp = await GetStoredOtp(userId);
        return storedOtp != null && storedOtp == otp;
    }
}
