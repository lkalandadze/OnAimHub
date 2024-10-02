using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.PlayerBanFeatures.Queries.Get;

public class GetBannedPlayersQuery : PagedRequest, IRequest<GetBannedPlayersQueryResponse>;