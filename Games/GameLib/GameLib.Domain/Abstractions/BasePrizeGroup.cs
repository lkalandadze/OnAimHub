#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Abstractions;

public abstract class BasePrizeGroup : BaseEntity<int>
{
    public List<int> Sequence { get; set; }
    public int? NextPrizeIndex { get; set; }

    public int ConfigurationId { get; set; }

    //TODO: leave this not mapped
    //[NotMapped]
    //public GameConfiguration Configuration { get; set; }

    public virtual List<BasePrize> GetBasePrizes() => [];
}

public abstract class BasePrizeGroup<TPrize> : BasePrizeGroup where TPrize : BasePrize
{
    public ICollection<TPrize> Prizes { get; set; }

    public override List<BasePrize> GetBasePrizes() =>
        Prizes.Select(x => x as BasePrize).ToList();
}