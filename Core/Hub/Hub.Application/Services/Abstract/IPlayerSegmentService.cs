namespace Hub.Application.Services.Abstract;

public interface IPlayerSegmentService
{
    Task AssignPlayersToSegmentAsync(IEnumerable<int> playerIds, string segmentId);

    void UnassignPlayersToSegment(IEnumerable<int> playerIds, string segmentId);
}