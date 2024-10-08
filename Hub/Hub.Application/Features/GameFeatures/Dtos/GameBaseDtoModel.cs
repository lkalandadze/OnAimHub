#nullable disable

using Hub.Application.Models.Game;

namespace Hub.Application.Features.GameFeatures.Dtos;

public class GameBaseDtoModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<GameConfigurationGetModel> Configurations { get; set; }

    public static GameBaseDtoModel MapFrom(ActiveGameModel game, IEnumerable<GameConfigurationGetModel> configurations = null)
    {
        return new GameBaseDtoModel
        {
            Id = game.Id,
            Name = game.Name,
            Configurations = configurations,
        };
    }
}