using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities;

public partial class Base
{
    public class Prize : BaseEntity
    {
        public override int Id { get; set; }
        public int Value { get; set; }
        public int Probability { get; set; }

        public int PrizeTypeId { get; set; }
        public PrizeType PrizeType { get; set; }

        public int PrizeGroupId { get; set; }
        public PrizeGroup PrizeGroup { get; set; }
    }

    public class WheelPrize
    {

    }
}