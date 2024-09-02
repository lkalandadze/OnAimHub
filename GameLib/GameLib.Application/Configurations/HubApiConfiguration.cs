#nullable disable

namespace GameLib.Application.Configurations;

public class HubApiConfiguration
{
    public string Host { get; set; }
    public HubEndpoints Endpoints { get; set; }
}

public class HubEndpoints
{
    public string BetTransaction { get; set; }
    public string WinTransaction { get; set; }
}