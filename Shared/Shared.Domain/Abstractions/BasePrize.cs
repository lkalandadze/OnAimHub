using Shared.Domain.Entities;

namespace Shared.Domain.Abstractions;

public abstract class BasePrize : BaseEntity
{
    public override int Id { get; set; }
    public int Value { get; set; }
    public int Probability { get; set; }

    public int PrizeTypeId { get; set; }
    public PrizeType PrizeType { get; set; }

    public int PrizeGroupId { get; set; }
    public BasePrizeGroup PrizeGroup { get; set; }
}