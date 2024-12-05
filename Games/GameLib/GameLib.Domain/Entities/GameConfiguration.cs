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

    public GameConfiguration(string name, int value, Guid? correlationId = null, int? templateId = null, IEnumerable<Price> prices = null)
    {
        Name = name;
        Value = value;
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

    [GlobalDescription("Correlation Id of the configuration")]
    public Guid? CorrelationId { get; set; }

    [GlobalDescription("Template Id of the configuration")]
    public int? FromTemplateId { get; set; }

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

    public GameConfiguration(string name, int value, Guid? correlationId = null, int? templateId = null, IEnumerable<Price> prices = null)
        : base(name, value, correlationId, templateId, prices)
    { 
    
    }
}