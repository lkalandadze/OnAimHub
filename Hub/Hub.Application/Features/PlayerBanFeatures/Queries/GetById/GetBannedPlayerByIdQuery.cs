using MediatR;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.GetById;

public record GetBannedPlayerByIdQuery(int Id) : IRequest<GetBannedPlayerByIdQueryResponse>;