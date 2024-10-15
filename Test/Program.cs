using System.Collections;
using System.Reflection;

var a = new A
{
    Bs = new List<B>
            {
                new B
                {
                    Cs = new List<C>
                    {
                        new C
                        {
                            Ds = new List<D> { new D(), new D() }
                        },
                        new C()
                    }
                },
                new B
                {
                    Cs = new List<C>
                    {
                        new C()
                    }
                }
            }
};

var addresses = AddressCollector.CollectAddresses(a);

foreach (var address in addresses)
{
    Console.WriteLine(address);
}

// end main ------------------------------------------

static class AddressCollector
{
    public static List<string> CollectAddresses(object obj, string basePath = "")
    {
        var addresses = new List<string>();

        if (obj == null) return addresses;

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            // Check if the property is a collection
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                // Get the collection items
                var collection = property.GetValue(obj) as IEnumerable;
                if (collection != null)
                {
                    int index = 0;
                    foreach (var item in collection)
                    {
                        string path = $"{basePath}.{property.Name}[{index}]";
                        addresses.Add(path);
                        addresses.AddRange(CollectAddresses(item, path));
                        index++;
                    }
                }
            }
        }

        return addresses;
    }
}

class A
{
    public ICollection<B> Bs { get; set; } = new List<B>();
}

class B
{
    public ICollection<C> Cs { get; set; } = new List<C>();
}

class C
{
    public ICollection<D> Ds { get; set; } = new List<D>();
}

class D
{
    // Additional properties for class D
}