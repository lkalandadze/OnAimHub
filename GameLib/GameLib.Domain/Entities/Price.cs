#nullable disable

using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

public class Price : BaseEntity<int>
{
    public Price()
    {
        
    }

    public Price(decimal value, decimal multiplier, string currencyId, int configurationId)
    {
        Value = value;
        Multiplier = multiplier;
        CurrencyId = currencyId;
        ConfigurationId = configurationId;
    }

    public decimal Value { get; private set; }
    public decimal Multiplier { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public int ConfigurationId { get; private set; }
    [JsonIgnore]
    public Configuration Configuration { get; private set; }
}