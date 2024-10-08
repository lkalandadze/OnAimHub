#nullable disable
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

public class Price : BaseEntity<string>
{
    public Price()
    {

    }

    public Price(decimal value, decimal multiplier, string currencyId)
    {
        Value = value;
        Multiplier = multiplier;
        CurrencyId = currencyId;
    }

    public decimal Value { get; private set; }
    public decimal Multiplier { get; private set; }
    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }
}

public class Price<T> : Price where T : Price<T> 
{
    public Price()
    {
        
    }
    public Price(decimal value, decimal multiplier, string currencyId, int configurationId) : base(value, multiplier, currencyId)
    {
        // Initialization logic for the generic segment, if any
    }

    [JsonIgnore]
    public T Configuration { get; private set; }
}