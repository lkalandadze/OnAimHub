using OnAim.Admin.Infrasturcture.Repository.Abstract;
using Shared.Application;
using Shared.Lib.Attributes;

namespace OnAim.Admin.APP.Services.Admin.SettingServices;

public class AppSettings : Settings
{
    private readonly IRepository<OnAim.Admin.Domain.Entities.User> _userRepository;

    public AppSettings(IAppSettingRepository appSettingRepository, IRepository<OnAim.Admin.Domain.Entities.User> userRepository)
        : base(appSettingRepository)
    {
        _userRepository = userRepository;
    }

    [SettingPropertyDefaultValue(false)]
    public SettingProperty<bool> TwoFactorAuth { get; set; }

    [SettingPropertyDefaultValue(false)]
    public SettingProperty<bool> DomainRestrictionsEnabled { get; set; }

    public async Task<bool> SetTwoFactorAuth(bool twoFactorAuth)
    {
        SetValue(nameof(TwoFactorAuth), twoFactorAuth);

        var users = await _userRepository.Query().ToListAsync();

        foreach (var user in users)
        {
            user.IsTwoFactorEnabled = twoFactorAuth;
            await _userRepository.CommitChanges();
        }

        return true;
    }
}