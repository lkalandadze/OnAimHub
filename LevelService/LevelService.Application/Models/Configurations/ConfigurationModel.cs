using LevelService.Domain.Entities;

namespace LevelService.Application.Models.Configurations;

public class ConfigurationModel
{
    public int Id { get; set; }
    public string CurrencyId { get; set; }
    public decimal ExperienceToGrant { get; set; }
    
    public static ConfigurationModel MapFrom(Configuration configuration)
    {
        return new ConfigurationModel
        {
            Id = configuration.Id,
            CurrencyId = configuration.CurrencyId,
            ExperienceToGrant = configuration.ExperienceToGrant,
        };
    }
}
