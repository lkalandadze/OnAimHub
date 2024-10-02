using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetGeneralSegmentActsHistory
{
    public class GetGeneralSegmentActsHistoryQueryHandler : IQueryHandler<GetGeneralSegmentActsHistoryQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<PlayerSegmentActHistory> _readOnlyRepository;

        public GetGeneralSegmentActsHistoryQueryHandler(IReadOnlyRepository<PlayerSegmentActHistory> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ApplicationResult> Handle(GetGeneralSegmentActsHistoryQuery request, CancellationToken cancellationToken)
        {
            var query = _readOnlyRepository.Query();

            //if (request.Filter.DateFrom.HasValue)
            //    query = query.Where(x => x.DateCreated >= request.Filter.DateFrom.Value);

            //if (request.Filter.DateTo.HasValue)
            //    query = query.Where(x => x.DateCreated <= request.Filter.DateTo.Value);

            //if (request.Filter.SegmentId != null)
            //    query = query.Where(x => x.se.Id == request.Filter.SegmentId);

            //if (request.Filter.UserId != null || request.Filter.UserId != 0)
            //{
            //    query = query.Where(x => x.ByUserId == request.Filter.UserId);
            //}

            if (request.Filter.UserId != null || request.Filter.UserId != 0)
            {
                query = query.Where(x => x.PlayerId == request.Filter.playerId);
            }

            var paginatedResult = await Paginator.GetPaginatedResult(
            query,
            request.Filter,
            act => new ActsHistoryDto
            {
                Id = act.Id,
                Note = null,
                PlayerName = act.Player.UserName,
                UploadedOn = null,
                Quantity = act.PlayerSegmentAct.TotalPlayers,
                Type = act.PlayerSegmentAct.Action.Name,
            },
            cancellationToken
        );

            return new ApplicationResult
            {
                Success = true,
                Data = paginatedResult,
            };
        }
    }
}
