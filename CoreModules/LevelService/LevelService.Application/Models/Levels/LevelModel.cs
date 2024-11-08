using LevelService.Application.Models.LevelPrizes;

namespace LevelService.Application.Models.Levels;

public class LevelModel
{
    public int Number { get; set; }
    public int ExperienceToArchive { get; set; }
    public List<LevelPrizesModel> Prizes { get; set; }
}
