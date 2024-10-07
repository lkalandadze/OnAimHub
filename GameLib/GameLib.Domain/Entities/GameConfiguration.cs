#nullable disable

using OnAim.Lib.EntityExtension.GlobalAttributes.Attributes;
using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public abstract class GameConfiguration : BaseEntity<int>
{
    public GameConfiguration()
    {

    }

    public GameConfiguration(string name, int value, IEnumerable<Price> prices = null, IEnumerable<Segment> segments = null)
    {
        Name = name;
        Value = value;
        //Prices = prices?.Select(x => x as Price<T>).ToList() ?? [];
        //Segments = segments?.Select(x => x as Segment<T>).ToList() ?? [];
    }

    [GlobalDescription("Name of the configuration")]
    public string Name { get; private set; }

    [GlobalDescription("Value of the configuration")]
    public int Value { get; private set; }

    [GlobalDescription("IsActive of the configuration")]
    public bool IsActive { get; private set; }
    public void ChangeDetails(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public void AssignSegments(IEnumerable<Segment> segments)
    {
        foreach (var segment in segments)
        {
            //Segments.Add(segment as Segment<T>);
        }
    }

    public void UnassignSegments(IEnumerable<Segment> segments)
    {
        foreach (var segment in segments)
        {
            //Segments.Remove(segment as Segment<T>);
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

    //[IgnoreIncludeAll]
    public ICollection<Price> Prices { get; set; }

    //[IgnoreIncludeAll]
    public ICollection<Segment> Segments { get; set; }
}

public abstract class GameConfiguration<T> : GameConfiguration where T : GameConfiguration<T>
{
    public GameConfiguration()
    {

    }

    public GameConfiguration(string name, int value, IEnumerable<Price> prices = null, IEnumerable<Segment> segments = null) : base(name, value, prices, segments)
    { 
    
    }

    //[IgnoreIncludeAll]
    //public new ICollection<Price> Prices { get;  set; }

    //[IgnoreIncludeAll]
    //public new ICollection<Segment<T>> Segments { get; private set; }
}