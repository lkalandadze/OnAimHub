namespace Shared.Application.Models.Consul;

public class GameRegisterResponseModel
{
    public int GameVersionId { get; set; }
    public string GameVersionName { get; set; }
    public bool GameVersionIsActive { get; set; }
    public string Address { get; set; }
    public List<int> SegmentIds { get; set; }
    public DateTime ActivationTime { get; set; }
    public List<ConfigurationResponseModel> Configurations { get; set; }
}

public class ConfigurationResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}