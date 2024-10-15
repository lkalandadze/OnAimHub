using Microsoft.EntityFrameworkCore.Metadata;

public class EntitySeedOptions
{
    public Type TargetType { get; set; }
    internal IEntityType TargetEntityType { get; set; }
    public List<PropertySeedOption> PropertySeedOptions { get; set; }
    public int SeedCount { get; set; } = 50;
}