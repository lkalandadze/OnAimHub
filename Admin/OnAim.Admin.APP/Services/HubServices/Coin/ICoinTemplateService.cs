using OnAim.Admin.APP.Services.Hub.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinTemplateService
{
    Task<ApplicationResult> GetAllCoinTemplates(BaseFilter filter);
    Task<ApplicationResult> GetCoinTemplateById(string id);
    Task<CoinTemplateListDto> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult> DeleteCoinTemplate(string CoinTemplateId);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
