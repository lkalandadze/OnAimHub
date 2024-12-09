using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinTemplateService
{
    Task<ApplicationResult> GetAllCoinTemplates();
    Task<ApplicationResult> GetCoinTemplateById(string id);
    Task<CoinTemplate> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult> DeleteCoinTemplate(string CoinTemplateId);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
