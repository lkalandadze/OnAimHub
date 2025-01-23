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

    public GameConfiguration(string name, int value, int promotionId, string description = null, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null)
    {
        Name = name;
        Description = description;
        Value = value;
        PromotionId = promotionId;
        CorrelationId = correlationId;
        FromTemplateId = templateId;
        Prices = prices?.ToList() ?? [];
    }

    [GlobalDescription("Name of the configuration")]
    public string Name { get; set; }

    [GlobalDescription("Description of the configuration")]
    public string Description { get; set; }

    [GlobalDescription("Value of the configuration")]
    public int Value { get; set; }

    [IgnoreIncludeAll]
    [GlobalDescription("IsActive of the configuration")]
    public bool IsActive { get; set; }

    [IgnoreIncludeAll]
    [GlobalDescription("Promotion Id of the configuration")]
    public int PromotionId { get; set; }

    [IgnoreIncludeAll]
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

    public GameConfiguration(string name, int value, int promotionId, string description = null, Guid? correlationId = null, string templateId = null, IEnumerable<Price> prices = null)
        : base(name, value, promotionId, description, correlationId, templateId, prices)
    { 
    
    }
}