using GameLib.Domain.Entities;
using Shared.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

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
        get { return base.Prizes.Select(x => (TPrize)x).ToList(); }
        set { base.Prizes = value.Select(x => (BasePrize)x).ToList(); }
    }
}