#nullable disable

namespace GameLib.Application.Models.Configuration;

public class ConfigurationGetModel : ConfigurationBaseGetModel
{
    public static ConfigurationGetModel MapFrom(Domain.Entities.GameConfiguration configuration, bool includeNavProperties = true)
    {
        var model = new ConfigurationGetModel
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Value = configuration.Value,
            IsActive = configuration.IsActive,
        };

        //if (includeNavProperties)
        //{
        //}

        return model;
    }
}