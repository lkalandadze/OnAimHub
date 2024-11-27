using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ICoinService
{
    Task<ApplicationResult> GetAllCoins(BaseFilter baseFilter);
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> CreateCoinTemplate(CreateCoinTemplateDto coinTemplate);
    Task<ApplicationResult> DeleteCoinTemplate(ObjectId CoinTemplateId);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateCoinTemplateDto update);
}
