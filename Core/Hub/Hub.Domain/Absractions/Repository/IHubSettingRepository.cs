using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Absractions.Repository;

public interface IHubSettingRepository : IBaseRepository<HubSetting>, ISettingRepository
{
}