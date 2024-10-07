#nullable disable

namespace GameLib.Application.Models.Configuration;

public class ConfigurationBaseGetModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }

    public static ConfigurationBaseGetModel MapFrom(Domain.Entities.GameConfiguration configuration)
    {
        return ConfigurationGetModel.MapFrom(configuration, false);
    }
}