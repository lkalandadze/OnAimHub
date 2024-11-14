#nullable disable

using CheckmateValidations;
using GameLib.Domain.Checkers;
using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

[CheckMate<GameConfigurationChecker>]
public class GameConfiguration : BaseEntity<int>
{
    public GameConfiguration()
    {

    }

    public GameConfiguration(string name, int value, IEnumerable<Price> prices = null)
    {
        Name = name;
        Value = value;
        Prices = prices?.ToList() ?? [];
    }

    [GlobalDescription("Name of the configuration")]
    public string Name { get; set; }

    [GlobalDescription("Value of the configuration")]
    public int Value { get; set; }

    [GlobalDescription("IsActive of the configuration")]
    public bool IsActive { get; set; }

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

    public GameConfiguration(string name, int value, IEnumerable<Price> prices = null) : base(name, value, prices)
    { 
    
    }

    //[IgnoreIncludeAll]
    public new ICollection<Price> Prices { get; set; }
}