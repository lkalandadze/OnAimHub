using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinTemplateService
{
    Task<ApplicationResult<PaginatedResult<CoinTemplateListDto>>> GetAllCoinTemplates(BaseFilter filter);
    Task<ApplicationResult<CoinTemplateDto>> GetCoinTemplateById(string id);
    Task<bool> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult<bool>> DeleteCoinTemplate(string CoinTemplateId);
    Task<ApplicationResult<bool>> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
