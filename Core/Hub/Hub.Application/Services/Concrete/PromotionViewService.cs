using Hub.Application.Configurations;
using Hub.Application.Services.Abstract;
using Microsoft.Extensions.Options;

namespace Hub.Application.Services.Concrete;

public class PromotionViewService : IPromotionViewService
{
    private readonly PromotionViewConfiguration _viewConfig;

    public PromotionViewService(IOptions<PromotionViewConfiguration> viewConfig)
    {
        _viewConfig = viewConfig.Value;
    }

    public string GenerateViewUrl(string viewContent, int promotionId)
    {
        return GenerateUrl(viewContent, _viewConfig.Directory, promotionId.ToString());
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

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), directory);
        var filePath = Path.Combine(uploadsDir, fileName);
        Directory.CreateDirectory(uploadsDir);

        File.WriteAllText(filePath, viewContent);

        return $"{_viewConfig.Host}/{directory}/{fileName}";
    }
}