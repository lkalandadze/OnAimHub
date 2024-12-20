using OnAim.Admin.Domain.HubEntities;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using System.Text.Json.Serialization;

namespace OnAim.Admin.Domain.GameEntities;

public class GameConfiguration : BaseEntity<int>
{
    public GameConfiguration()
    {

    }

    public GameConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null)
    {
        Name = name;
        Value = value;
        PromotionId = promotionId;
        CorrelationId = correlationId;
        FromTemplateId = templateId;
        Prices = prices?.ToList() ?? [];
    }

    [GlobalDescription("Name of the configuration")]
    public string Name { get; set; }

    [GlobalDescription("Value of the configuration")]
    public int Value { get; set; }

    [GlobalDescription("IsActive of the configuration")]
    public bool IsActive { get; set; }

    [GlobalDescription("Promotion Id of the configuration")]
    public int PromotionId { get; set; }

    [GlobalDescription("Correlation Id of the configuration")]
    public Guid? CorrelationId { get; set; }

    [GlobalDescription("Template Id of the configuration")]
    public string FromTemplateId { get; set; }

    //[IgnoreIncludeAll]
    public ICollection<Price> Prices { get; set; }

    public void ChangeDetails(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

public class GameConfiguration<T> : GameConfiguration where T : GameConfiguration<T>
{
    public GameConfiguration()
    {

    }

    public GameConfiguration(string name, int value, int promotionId, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null)
        : base(name, value, promotionId, correlationId, templateId, prices)
    {

    }
}
public class Price : BaseEntity<string>
{
    public Price()
    {

    }

    public Price(decimal value, decimal multiplier, string coinId)
    {
        Value = value;
        Multiplier = multiplier;
        CoinId = coinId;
    }

    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }
    public string CoinId { get; set; }

    //[JsonIgnore]
    //public Coin Coin { get; set; }
}

public class Price<T> : Price where T : Price<T>
{
    public Price()
    {

    }
    public Price(decimal value, decimal multiplier, string coinId, int configurationId) : base(value, multiplier, coinId)
    {
        // Initialization logic for the generic segment, if any
    }

    [JsonIgnore]
    public T Configuration { get; set; }
}