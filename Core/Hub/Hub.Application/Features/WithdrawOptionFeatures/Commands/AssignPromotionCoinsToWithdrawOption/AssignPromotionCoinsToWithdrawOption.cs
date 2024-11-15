using MediatR;

namespace Hub.Application.Features.WithdrawOptionFeatures.Commands.AssignPromotionCoinsToWithdrawOption;

public record AssignPromotionCoinsToWithdrawOption(int WithdrawOptionId, IEnumerable<string> PromotionCoinIds) : IRequest;