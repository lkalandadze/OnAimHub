using MediatR;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPromoCode;

public sealed record GetPromoCodeQuery : IRequest<string>;
