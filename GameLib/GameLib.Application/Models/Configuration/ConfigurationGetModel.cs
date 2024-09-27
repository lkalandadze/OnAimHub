#nullable disable

namespace GameLib.Application.Models.Configuration;

public class ConfigurationGetModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
}