#nullable disable

using CheckmateValidations;
using GameLib.Domain.Checkers;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;
using System.Text.Json.Serialization;

namespace GameLib.Domain.Entities;

[CheckMate<PriceChecker>]
public class Price : BaseEntity<int>
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

    [GlobalDescription("Value of the price")]
    public decimal Value { get; set; }

    [GlobalDescription("Multiplier of the price")]
    public decimal Multiplier { get; set; }

    [GlobalDescription("CoinId of the price")]
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

    [IgnoreIncludeAll]
    [JsonIgnore]
    public T Configuration { get; set; }
}