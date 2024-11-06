#nullable disable

using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities.DbEnums;
using Shared.Application;
using Shared.Lib.Attributes;

namespace Hub.Application;

public class HubSettings(IHubSettingRepository settingRepository) : Settings(settingRepository)
{
    [SettingPropertyDefaultValue(50)]
    public SettingProperty<int> MaxReferralsPerUser { get; set; }

    [SettingPropertyDefaultValue(nameof(Currency.FreeSpin))]
    public SettingProperty<string> ReferrerPrizeCurrencyId { get; set; }

    [SettingPropertyDefaultValue(nameof(Currency.FreeSpin))]
    public SettingProperty<string> ReferralPrizeCurrencyId { get; set; }

    [SettingPropertyDefaultValue(100)]
    public SettingProperty<int> ReferrerPrizeAmount { get; set; }

    [SettingPropertyDefaultValue(50)]
    public SettingProperty<int> ReferralPrizeAmount { get; set; }
}