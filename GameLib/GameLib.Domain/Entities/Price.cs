﻿using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Price : BaseEntity<int>
{
    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public int SegmentId { get; set; }
    public Segment Segment { get; set; }

    public int ConfigurationId { get; set; }
    public Configuration Configuration { get; set; }
}