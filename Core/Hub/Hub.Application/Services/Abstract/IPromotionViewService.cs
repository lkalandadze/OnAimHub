namespace Hub.Application.Services.Abstract;

public interface IPromotionViewService
{
    string GenerateViewUrl(string viewContent, int promotionId);

    string GenerateTemplateViewUrl(string viewContent);
}