using Hub.Domain.Absractions;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class SettingRepository(HubDbContext context) : BaseRepository<HubDbContext, Setting>(context), ISettingRepository
{
    public List<Setting> GetAllSettings()
    {
        return _context.Settings.ToList();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}