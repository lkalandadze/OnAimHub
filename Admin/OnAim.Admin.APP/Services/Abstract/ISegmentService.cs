using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ISegmentService
{
    Task<ApplicationResult> AssignSegmentToPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult> AssignSegmentToPlayer(string segmentId, int playerId);
    Task<ApplicationResult> BlockSegmentForPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult> BlockSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult> CreateSegment(string id, string description, int priorityLevel);
    Task<ApplicationResult> DeleteSegment(string id);
    Task<ApplicationResult> UnAssignPlayersToSegment(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult> UnAssignSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult> UnBlockSegmentForPlayer(string segmentId, int playerId);
    Task<ApplicationResult> UnBlockSegmentForPlayers(IEnumerable<string> segmentId, IFormFile file);
    Task<ApplicationResult> UpdateSegment(string id, string description, int priorityLevel);
    Task<ApplicationResult> GetAll(int? pageNumber, int? pageSize);
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> GetActivePlayers(string segmentId, FilterBy filter);
    Task<ApplicationResult> GetBlackListedPlayers(string segmentId, FilterBy filter);
    Task<ApplicationResult> GetActs(string segmentId);
    Task<ApplicationResult> GetActsHistory(int playerSegmentActId);
    Task<ApplicationResult> GetGeneralSegmentActs(SegmentActsFilter filter);
    Task<ApplicationResult> GetGeneralSegmentActsHistory(SegmentActsFilter filter);
}