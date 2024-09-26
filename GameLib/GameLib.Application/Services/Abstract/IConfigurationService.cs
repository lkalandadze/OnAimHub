namespace GameLib.Application.Services.Abstract;

public interface IConfigurationService
{
    Task AssignConfigurationToSegments(int configurationId, IEnumerable<string> segmentIds);
}