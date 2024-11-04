namespace Shared.Lib.Helpers;

public static class CollectionHelper
{
    public static void RemoveFromCollection(object collection, object itemToRemove)
    {
        var collectionType = collection.GetType();
        var isHashSet = collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(HashSet<>);

        if (!isHashSet)
        {
            throw new ArgumentException("The provided object is not a HashSet.");
        }

        var itemType = collectionType.GetGenericArguments()[0];

        if (!itemType.IsInstanceOfType(itemToRemove))
        {
            throw new ArgumentException($"The item to remove is not of the correct type. Expected type: {itemType}");
        }

        var removeMethod = collectionType.GetMethod(nameof(ICollection<object>.Remove));

        if (removeMethod != null)
        {
            removeMethod.Invoke(collection, [itemToRemove]);
        }
    }

    public static void AddToCollection(object collection, object itemToAdd)
    {
        var collectionType = collection.GetType();
        var isHashSet = collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(HashSet<>);

        if (!isHashSet)
        {
            throw new ArgumentException("The provided object is not a HashSet.");
        }

        var itemType = collectionType.GetGenericArguments()[0];

        if (!itemType.IsInstanceOfType(itemToAdd))
        {
            throw new ArgumentException($"The item to add is not of the correct type. Expected type: {itemType}");
        }

        var removeMethod = collectionType.GetMethod(nameof(ICollection<object>.Add));

        if (removeMethod != null)
        {
            removeMethod.Invoke(collection, [itemToAdd]);
        }
    }
}