using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public partial class Base
{
    public class PrizeGroup : BaseEntity
    {
        public override int Id { get; set; }
        public List<int> Sequence { get; set; }
        public int? NextPrizeIndex { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }

        public ICollection<Prize> Prizes { get; set; }
    }
}