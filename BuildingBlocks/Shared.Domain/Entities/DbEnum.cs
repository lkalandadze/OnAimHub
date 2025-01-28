#nullable disable

namespace Shared.Domain.Entities;

public class DbEnum<T> : BaseEntity<T>
{
    public string Name { get; set; }
}

public class DbEnum<T, U> : DbEnum<T> where U : DbEnum<T>, new()
{
    public static bool operator ==(DbEnum<T, U> obj1, DbEnum<T, U> obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(DbEnum<T, U> obj1, DbEnum<T, U> obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object obj)
    {
        return (obj as DbEnum<T, U>)?.Id.Equals(Id) ?? false;
    }

    public static U FromId(T id, string name = null)
    {
        return new U { Id = id, Name = name };
    }
}