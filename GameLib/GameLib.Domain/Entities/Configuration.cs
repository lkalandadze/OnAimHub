#nullable disable

using GameLib.Domain.Generators;
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

    public string Rule { get; set; }
    [MetaDescription("Name of the configuration")]
    public string Name { get; private set; }
    [MetaDescription("Value of the configuration")]
    public int Value { get; private set; }
    [MetaDescription("IsActive of the configuration")]
    public bool IsActive { get; private set; }

    public ICollection<Price> Prices { get; private set; }
    public ICollection<Segment> Segments { get; private set; }

    public void ChangeDetails(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public void AssignSegments(IEnumerable<Segment> segments)
    {
        foreach (var segment in segments)
        {
            Segments.Add(segment);
        }
    }

    public void UnassignSegments(IEnumerable<Segment> segments)
    {
        foreach (var segment in segments)
        {
            Segments.Remove(segment);
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