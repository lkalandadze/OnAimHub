using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ICoinService
{
    Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter);
    Task<ApplicationResult> GetById(string id);
}
