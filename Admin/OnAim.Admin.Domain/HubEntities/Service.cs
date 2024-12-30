namespace OnAim.Admin.Domain.HubEntities;

public class Service : BaseEntity<int>
{
    public Service(string type, string name, bool isActive)
    {
        Type = type;
        Name = name;
        IsActive = isActive;
    }

    public string Type { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }


    public ICollection<Promotion> Promotions { get; private set; } = new List<Promotion>();
}