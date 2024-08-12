using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetBalance;

public class GetBalanceRequest : IRequest<GetBalanceResponse>
{
    public int PlayerId { get; set; }
}