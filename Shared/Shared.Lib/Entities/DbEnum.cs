namespace Shared.Lib.Entities;

public class DbEnum : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }

    public static bool operator ==(DbEnum obj1, DbEnum obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(DbEnum obj1, DbEnum obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object? obj)
    {
        return (obj as DbEnum)?.Id == Id;
    }
}
