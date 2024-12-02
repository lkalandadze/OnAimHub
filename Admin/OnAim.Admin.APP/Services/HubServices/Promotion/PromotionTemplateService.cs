using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public class PromotionTemplateService : IPromotionTemplateService
{
    private readonly IPromotionTemplateRepository _promotionTemplateRepository;

    public PromotionTemplateService(IPromotionTemplateRepository promotionTemplateRepository)
    {
        _promotionTemplateRepository = promotionTemplateRepository;
    }

    public async Task<ApplicationResult> GetAllTemplates()
    {
        var temps = await _promotionTemplateRepository.GetPromotionTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var coin = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> CreatePromotionTemplate(PromotionTemplate template)
    {
        var temp = new PromotionTemplate
        {
            Title = template.Title,
            StartDate = template.StartDate,
            Description = template.Description,
            EndDate = template.EndDate,
            CoinIds = template.CoinIds,
            SegmentIds = template.SegmentIds,
            Status = template.Status,
            TotalCost = template.TotalCost,
        };

       

        await _promotionTemplateRepository.AddPromotionTemplateAsync(temp);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeletePromotionTemplate(string CoinTemplateId)
    {
        //var coinTemplate = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(CoinTemplateId);

        //if (coinTemplate == null)
        //{
        //    throw new NotFoundException("Coin Template Not Found");
        //}

        //coinTemplate.Delete();

        //await _coinRepository.UpdateCoinTemplateAsync(CoinTemplateId, coinTemplate);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdatePromotionTemplate(UpdateCoinTemplateDto update)
    {
        //var coinTemplate = await _promotionTemplateRepository.GetPromotionTemplateByIdAsync(update.Id);

        //if (coinTemplate == null || coinTemplate.IsDeleted)
        //{
        //    throw new NotFoundException($"Coin template with the specified ID: [{update.Id}] was not found.");
        //}

        return new ApplicationResult { Success = true };
    }
}
