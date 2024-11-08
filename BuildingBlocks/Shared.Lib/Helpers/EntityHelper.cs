namespace Shared.Lib.Helpers;

public static class EntityHelper
{
    public static bool IsNewEntity(object entity)
    {
        var id = GetId(entity);
        return (id is int intId && intId == 0) || (id is string strId && string.IsNullOrEmpty(strId));
    }

    public static bool HaveSameId(object entity1, object entity2)
    {
        return GetId(entity1) == GetId(entity2);
    }

    public static dynamic? GetId(object entity)
    {
        return entity.GetType().GetProperties().FirstOrDefault(x => x.Name == "Id")?.GetValue(entity);
    }
}