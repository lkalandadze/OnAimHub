using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;

namespace Hub.Domain.Abstractions.Repository;

public interface IHubSettingRepository : IBaseRepository<HubSetting>, ISettingRepository
{
}