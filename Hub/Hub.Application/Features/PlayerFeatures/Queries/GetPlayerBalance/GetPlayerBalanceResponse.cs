using Hub.Application.Features.PlayerFeatures.Dtos;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;

public class GetPlayerBalanceResponse
{
    public IEnumerable<PlayerBalanceBaseDtoModel>? Balances { get; set; }
    public int PlayerId { get; set; }
}