#nullable disable

using Hub.Application.Models.Game;

namespace Hub.Application.Features.GameFeatures.Dtos;

public class GameBaseDtoModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public int ConfigurationCount { get; set; }
    public IEnumerable<int> PromotionIds { get; set; }

    public static GameBaseDtoModel MapFrom(GameModel model, GameShortInfoGetModel game)
    {
        return new GameBaseDtoModel
        {
            Id = model.Id,
            Name = model.Name,
            Address = model.Address,
            Status = game.Status,
            Description = game.Description,
            ConfigurationCount = game.ConfigurationCount,
            PromotionIds = game.PromotionIds,
        };
    }
}