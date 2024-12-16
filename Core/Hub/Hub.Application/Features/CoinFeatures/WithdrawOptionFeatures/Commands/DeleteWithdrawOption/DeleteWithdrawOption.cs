using MediatR;

namespace Hub.Application.Features.CoinFeatures.WithdrawOptionFeatures.Commands.DeleteWithdrawOption;

public record DeleteWithdrawOption(int Id) : IRequest;