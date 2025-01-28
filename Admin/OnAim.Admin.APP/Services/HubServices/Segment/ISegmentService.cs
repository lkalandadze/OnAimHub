using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.APP.Services.HubServices.Segment;

public interface ISegmentService
{
    Task<ApplicationResult<object>> AssignSegmentToPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult<bool>> AssignSegmentToPlayer(string segmentId, int playerId);
    Task<ApplicationResult<object>> BlockSegmentForPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult<bool>> BlockSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult<bool>> CreateSegment(string id, string description, int priorityLevel);
    Task<ApplicationResult<bool>> DeleteSegment(string id);
    Task<ApplicationResult<object>> UnAssignPlayersToSegment(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult<bool>> UnAssignSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult<bool>> UnBlockSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult<object>> UnBlockSegmentForPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult<bool>> UpdateSegment(string id, string description, int priorityLevel);
    Task<ApplicationResult<PaginatedResult<SegmentListDto>>> GetAll(int? pageNumber, int? pageSize);
    Task<ApplicationResult<SegmentDto>> GetById(string id);
    Task<ApplicationResult<PaginatedResult<SegmentPlayerDto>>> GetActivePlayers(string segmentId, FilterBy filter);
    Task<ApplicationResult<PaginatedResult<SegmentPlayerDto>>> GetBlackListedPlayers(string segmentId, FilterBy filter);
    Task<ApplicationResult<IEnumerable<ActsDto>>> GetActs(string segmentId);
    Task<ApplicationResult<IEnumerable<ActsHistoryDto>>> GetActsHistory(int playerSegmentActId);
    Task<ApplicationResult<PaginatedResult<ActsDto>>> GetGeneralSegmentActs(SegmentActsFilter filter);
    Task<ApplicationResult<PaginatedResult<ActsHistoryDto>>> GetGeneralSegmentActsHistory(SegmentActsFilter filter);
}