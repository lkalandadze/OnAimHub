using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOptionGroup;

public record DeleteWithdrawOptionGroup(int Id) : IRequest;