using Hub.Domain.Enum;
using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.CreateWithdrawOption;

public record CreateWithdrawOption(string Title, string Description, string ImageUrl, EndpointContentType ContentType, IEnumerable<string> PromotionCoinIds, IEnumerable<int> CoinTemplateIds, string Endpoint, string EndpointContent, int? TemplateId) : IRequest;