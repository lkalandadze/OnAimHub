using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.ApplyPromoCode;

public record ApplyPromoCodeCommand(string ReferralCode) : IRequest<Unit>;