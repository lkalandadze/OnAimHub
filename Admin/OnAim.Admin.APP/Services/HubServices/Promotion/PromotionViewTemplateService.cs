﻿namespace OnAim.Admin.APP.Services.HubServices.Promotion;

public class PromotionViewTemplateService : IPromotionViewTemplateService
{
    private readonly IPromotionViewTemplateRepository _promotionViewTemplateRepository;
    private readonly PromotionViewConfiguration _viewConfig;

    public PromotionViewTemplateService(IPromotionViewTemplateRepository promotionViewTemplateRepository, IOptions<PromotionViewConfiguration> viewConfig)
    {
        _promotionViewTemplateRepository = promotionViewTemplateRepository;
        _viewConfig = viewConfig.Value;
    }

    public async Task<ApplicationResult<PaginatedResult<PromotionViewTemplate>>> GetAllPromotionViewTemplates(BaseFilter filter)
    {
        var temps = await _promotionViewTemplateRepository.GetPromotionViewTemplates();
        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    temps = temps.Where(u => u.IsDeleted == false).ToList();
                    break;
                case HistoryStatus.Deleted:
                    temps = temps.Where(u => u.IsDeleted == true).ToList();
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }
        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = temps
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PromotionViewTemplate>>
        {
            Success = true,
            Data = new PaginatedResult<PromotionViewTemplate>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult<PromotionViewTemplate>> GetPromotionViewTemplateById(string id)
    {
        var coin = await _promotionViewTemplateRepository.GetPromotionViewTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("template Not Found");

        return new ApplicationResult<PromotionViewTemplate> { Success = true, Data = coin };
    }

    public async Task<ApplicationResult<bool>> CreatePromotionViewTemplateAsync(CreatePromotionViewTemplateAsyncDto create)
    {
        var viewUrl = GenerateTemplateViewUrl(create.ViewContent);

        var promotionViewTemplate = new PromotionViewTemplate
        {
            Name = create.Name,
            Url = viewUrl
        };

        await _promotionViewTemplateRepository.AddPromotionViewTemplateAsync(promotionViewTemplate);


        return new ApplicationResult<bool> { Success = true};
    }

    public async Task<ApplicationResult<bool>> DeletePromotionViewTemplate(string id)
    {
        var template = await _promotionViewTemplateRepository.GetPromotionViewTemplateByIdAsync(id);

        if (template == null)
        {
            throw new NotFoundException("Template Not Found");
        }

        template.Delete();

        await _promotionViewTemplateRepository.UpdatePromotionViewTemplateAsync(id, template);

        return new ApplicationResult<bool> { Success = true };
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