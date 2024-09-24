#nullable disable

using System.Collections.Concurrent;

namespace OnAim.Admin.Infrasturcture.HubEntities;

public class DbEnum<T> : BaseEntity<T>
{
    public string Name { get; set; }
    private static ConcurrentDictionary<T, DbEnum<T>> Instances = [];
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
        if (!Instances.TryGetValue(id, out var result)) {
            result = new DbEnum<T> { Id = id };
            Instances.TryAdd(id, result);
        }

        return result;
    } 
}
