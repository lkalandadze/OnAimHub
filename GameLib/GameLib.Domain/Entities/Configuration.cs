using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Configuration : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public int GameVersionId { get; set; }
    public GameVersion GameVersion { get; set; }

    //public ICollection<BasePrizeGroup> PrizeGroups { get; set; }
    //public ICollection<BasePrize> Prizes { get; set; }

    public void Update(string name, bool isDefault, bool isActive, int gameVersionId)
    {
        Name = name;
        IsDefault = isDefault;
        IsActive = isActive;
        GameVersionId = gameVersionId;
    }
}