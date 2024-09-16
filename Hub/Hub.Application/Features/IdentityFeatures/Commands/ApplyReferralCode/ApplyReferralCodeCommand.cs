using MediatR;

namespace Hub.Application.Features.IdentityFeatures.Commands.ApplyReferralCode;

public record ApplyReferralCodeCommand(string ReferralCode) : IRequest<Unit>;