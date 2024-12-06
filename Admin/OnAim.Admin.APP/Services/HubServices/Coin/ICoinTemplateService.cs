using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public interface ICoinTemplateService
{
    Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter);
    Task<ApplicationResult> GetById(string id);
    Task<CoinTemplate> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult> DeleteCoinTemplate(string CoinTemplateId);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
