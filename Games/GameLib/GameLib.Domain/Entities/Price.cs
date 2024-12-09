#nullable disable

using CheckmateValidations;
using GameLib.Domain.Checkers;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

[CheckMate<PriceChecker>]
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


    [JsonIgnore]
    public Coin Coin { get; set; }
    public string CoinId { get; set; }
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