using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Domain.Absractions;

public interface ISettingRepository : IBaseRepository<Setting>
{
    List<Setting> GetAllSettings();

    void SaveChanges();
}
