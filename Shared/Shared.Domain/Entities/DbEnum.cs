#nullable disable

namespace Shared.Domain.Entities;

public class DbEnum<T> : BaseEntity<T>
{
    public string Name { get; set; }

    public static bool operator ==(DbEnum<T> obj1, DbEnum<T> obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(DbEnum<T> obj1, DbEnum<T> obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object obj)
    {
        return (obj as DbEnum<T>)?.Id.Equals(Id)??false;
    }

    public static DbEnum<T> FromId(T id)
    {
        return new DbEnum<T> { Id = id };
    } 
}
