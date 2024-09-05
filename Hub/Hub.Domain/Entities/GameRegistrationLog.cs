using Shared.Domain.Entities;

namespace Hub.Domain.Entities;

public class GameRegistrationLog : BaseEntity<int>
{
    public GameRegistrationLog(int gameVersionId, string eventType, DateTimeOffset timeStamp, string? additionalData)
    {
        GameVersionId = gameVersionId;
        EventType = eventType;
        TimeStamp = timeStamp;
        AdditionalData = additionalData;
    }
    public int GameVersionId { get; set; }
    public string EventType { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string? AdditionalData { get; set; }
}
