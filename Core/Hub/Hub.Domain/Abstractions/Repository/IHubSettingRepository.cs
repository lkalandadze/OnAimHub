using Hub.Domain.Entities;
using Shared.Domain.Abstractions.Repository;

namespace Hub.Domain.Abstractions.Repository;

public interface IHubSettingRepository : IBaseEntityRepository<HubSetting>, ISettingRepository
{
}