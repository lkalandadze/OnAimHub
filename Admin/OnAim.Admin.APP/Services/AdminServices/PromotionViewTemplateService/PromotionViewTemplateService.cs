using Microsoft.Extensions.Options;
using MongoDB.Bson;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.Admin.PromotionViewTemplateService;

public class PromotionViewTemplateService : IPromotionViewTemplateService
{
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;
    private readonly PromotionViewConfiguration _viewConfig;

    public PromotionViewTemplateService(IPromotionViewTemplateRepository promotionViewTemplateRepository, IOptions<PromotionViewConfiguration> viewConfig)
    {
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _viewConfig = viewConfig.Value;
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
        var viewUrl = GenerateTemplateViewUrl(create.ViewContent);

        var promotionViewTemplate = new PromotionViewTemplate
        {
            Name = create.Name,
            Url = viewUrl
        };

        await _promotionViewTemplateRepository.AddPromotionViewTemplateAsync(promotionViewTemplate);


        return new ApplicationResult();
    }

    public string GenerateTemplateViewUrl(string viewContent)
    {
        return GenerateUrl(viewContent, _viewConfig.TemplateDirectory);
    }

    private string GenerateUrl(string viewContent, string directory, string? additionalFileName = null)
    {
        var fileName = $"{Guid.NewGuid()}.html";

        if (!string.IsNullOrEmpty(additionalFileName))
        {
            fileName = $"{additionalFileName}_{fileName}";
        }

        var localPath = new Uri(_viewConfig.Host).LocalPath;
        var uploadsDir = Path.Combine(localPath, directory);

        var filePath = Path.Combine(uploadsDir, fileName);
        Directory.CreateDirectory(uploadsDir);

        File.WriteAllText(filePath, viewContent);

        return $"{_viewConfig.Host}/{directory}/{fileName}";
    }
}