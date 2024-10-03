#nullable disable

namespace Hub.Application.Configurations;

public class GameApiConfiguration
{
    public string Host { get; set; }
    public GameEndpoints Endpoints { get; set; }
}

public class GameEndpoints
{
    public string GetGameConfigurations { get; set; }
}