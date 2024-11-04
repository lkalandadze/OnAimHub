using Shared.Domain.Entities;

namespace GameLib.Domain.Abstractions;

public abstract class BasePrizeGroup : BaseEntity<int>
{
    public List<int> Sequence { get; set; }
    public int? NextPrizeIndex { get; set; }

    public int ConfigurationId { get; set; }

    //todo
    // leave this not mapped
    //[NotMapped]
    //public GameConfiguration Configuration { get; set; }

    public ICollection<BasePrize> Prizes { get; set; }
}

public abstract class BasePrizeGroup<TPrize> : BasePrizeGroup where TPrize : BasePrize
{
    new public ICollection<TPrize> Prizes
    {
        get { return base.Prizes?.Select(x => (TPrize)x).ToList() ?? new List<TPrize>(); }
        set { base.Prizes = value?.Select(x => (BasePrize)x).ToList() ?? new List<BasePrize>(); }
    }
}