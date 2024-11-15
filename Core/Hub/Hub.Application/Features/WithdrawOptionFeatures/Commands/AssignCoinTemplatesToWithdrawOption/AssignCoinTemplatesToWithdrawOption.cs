using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignCoinTemplatesToWithdrawOption;

public record AssignCoinTemplatesToWithdrawOption(int WithdrawOptionId, IEnumerable<int> CoinTemplateIds) : IRequest;