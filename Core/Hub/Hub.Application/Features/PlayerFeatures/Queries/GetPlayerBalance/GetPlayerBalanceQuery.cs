using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;

public class GetPlayerBalanceQuery : IRequest<GetPlayerBalanceResponse>
{
    public int? PromotionId { get; set; }
}