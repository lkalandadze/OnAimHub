using MongoDB.Bson;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.PromotionViewTemplateService;

public class PromotionViewTemplateService : IPromotionViewTemplateService
{
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;

    public PromotionViewTemplateService(IPromotionViewTemplateRepository promotionViewTemplateRepository)
    {
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
    }

    public async Task<ApplicationResult> GetAllWithdrawEndpointTemplates()
    {
        var temps = await _promotionViewTemplateRepository.GetPromotionViewTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(ObjectId id)
    {
        var coin = await _promotionViewTemplateRepository.GetPromotionViewTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create)
    {
        //var viewUrl = _promotionViewTemplateRepository.GenerateTemplateViewUrl(request.ViewContent);

        var promotionViewTemplate = new PromotionViewTemplate
        {
            Name = create.Name,
            Url = ""
        };

        await _promotionViewTemplateRepository.AddPromotionViewTemplateAsync(promotionViewTemplate);


        return new ApplicationResult();
    }
}
public record CreatePromotionViewTemplateAsyncDto(string Name, string ViewContent);