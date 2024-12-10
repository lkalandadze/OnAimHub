using MongoDB.Bson;

namespace OnAim.Admin.Domain.Entities.Templates;

public class GameConfigurationTemplate
{
    public GameConfigurationTemplate(){}

    public GameConfigurationTemplate(string configurationJson)
    {
        Id = ObjectId.GenerateNewId().ToString();
        ConfigurationJson = configurationJson;
    }

    public string Id { get; set; }
    public string ConfigurationId { get; set; }
    public string ConfigurationJson { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
