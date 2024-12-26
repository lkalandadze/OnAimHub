using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

public class GameConfigDto
{
    public string GameName { get; set; }
    public GameConfigurationDto GameConfiguration { get; set; }
}
