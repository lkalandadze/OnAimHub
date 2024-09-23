using MediatR;
using Shared.Lib.Wrappersl;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.Get;

public class GetBannedPlayersQuery : PagedRequest, IRequest<GetBannedPlayersQueryResponse>;