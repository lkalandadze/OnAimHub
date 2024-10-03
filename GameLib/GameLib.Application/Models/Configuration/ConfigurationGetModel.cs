#nullable disable

using GameLib.Application.Models.Currency;
using GameLib.Application.Models.PrizeType;
using GameLib.Application.Models.Segment;

namespace GameLib.Application.Models.Configuration;

public class ConfigurationGetModel : ConfigurationBaseGetModel
{
    public IEnumerable<SegmentBaseGetModel> Segments { get; set; }

    public static ConfigurationGetModel MapFrom(Domain.Entities.Configuration configuration, bool includeNavProperties = true)
    {
        var model = new ConfigurationGetModel
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Value = configuration.Value,
            IsActive = configuration.IsActive,
        };

        if (includeNavProperties)
        {
            model.Segments = configuration.Segments != null ? configuration.Segments.Select(s => SegmentBaseGetModel.MapFrom(s)) : default;
        }

        return model;
    }
}