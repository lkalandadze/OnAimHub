using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnAim.Admin.Domain.Entities.Templates;

public class GameConfigurationTemplate
{
    public GameConfigurationTemplate(){}

    public GameConfigurationTemplate(string configurationJson)
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public List<Price> Prices { get; set; } = new List<Price>();
    public List<Round> Rounds { get; set; } = new List<Round>();
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
public class Price
{
    public string Id { get; set; }
    public int Value { get; set; }
    public int Multiplier { get; set; }
    public string CoinId { get; set; }
}

public class Round
{
    public int Id { get; set; }
    public List<int> Sequence { get; set; } = new List<int>();
    public int NextPrizeIndex { get; set; }
    public int ConfigurationId { get; set; }
    public List<Prize> Prizes { get; set; } = new List<Prize>();
    public string Name { get; set; }
}

public class Prize
{
    public int Id { get; set; }
    public int Value { get; set; }
    public int Probability { get; set; }
    public int PrizeTypeId { get; set; }
    public int PrizeGroupId { get; set; }
    public string Name { get; set; }
    public int WheelIndex { get; set; }
}