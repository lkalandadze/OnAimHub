using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinTemplateService
{
    Task<ApplicationResult<PaginatedResult<CoinTemplateListDto>>> GetAllCoinTemplates(BaseFilter filter);
    Task<ApplicationResult<CoinTemplateDto>> GetCoinTemplateById(string id);
    Task<bool> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult<bool>> DeleteCoinTemplate(string CoinTemplateId);
    Task<ApplicationResult<bool>> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
