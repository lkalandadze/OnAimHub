using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOptionEndpoint;

public record DeleteWithdrawOptionEndpoint(IEnumerable<int> Ids) : IRequest;