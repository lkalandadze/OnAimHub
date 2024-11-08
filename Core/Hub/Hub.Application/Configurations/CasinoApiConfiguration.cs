#nullable disable

namespace Hub.Application.Configurations;

public class CasinoApiConfiguration
{
    public string Host { get; set; }
    public CasinoEndpoints Endpoints { get; set; }
}

public class CasinoEndpoints
{
    public string GetPlayer { get; set; }
    public string GetBalance { get; set; }
}