
namespace Hub.Application.Services.Abstract;

public interface IPlayerBlockedSegmentService
{
    Task BlockPlayerSegmentAsync(IEnumerable<int> playerIds, string segmentId);
    void UnblockPlayerSegment(IEnumerable<int> playerIds, string segmentId);
}