﻿using Shared.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain.Abstractions;

public abstract class BasePrize : BaseEntity
{
    public override int Id { get; set; }
    public int Value { get; set; }
    public int Probability { get; set; }

    public int PrizeTypeId { get; set; }
    
    [NotMapped]
    public PrizeType PrizeType { get; set; }

    public int PrizeGroupId { get; set; }

}

public abstract class BasePrize<TPrizeGroup> : BasePrize
where TPrizeGroup : BasePrizeGroup
{
    public TPrizeGroup PrizeGroup { get; set; }
}