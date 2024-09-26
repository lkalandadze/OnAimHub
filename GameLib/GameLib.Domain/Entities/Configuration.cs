﻿#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class Configuration : BaseEntity<int>
{
    public Configuration()
    {
        
    }

    public Configuration(string name, int value, IEnumerable<Price> prices = null, IEnumerable<Segment> segments = null)
    {
        Name = name;
        Value = value;
        Prices = prices?.ToList() ?? [];
        Segments = segments?.ToList() ?? [];
    }

    public string Name { get; private set; }
    public int Value { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<Price> Prices { get; private set; }
    public ICollection<Segment> Segments { get; private set; }

    public void AddSegments(IEnumerable<Segment> segments)
    {
        foreach (var segment in segments)
        {
            Segments.Add(segment);
        }
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}