using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOptionEndpoint;

public record DeleteWithdrawOptionEndpoint(int Id) : IRequest;