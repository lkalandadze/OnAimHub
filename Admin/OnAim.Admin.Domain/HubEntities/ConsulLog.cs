namespace OnAim.Admin.Domain.HubEntities;

public class ConsulLog : BaseEntity<int>
{
    public ConsulLog(int gameId, string serviceName, int port, DateTimeOffset registration)
    {
        GameId = gameId;
        ServiceName = serviceName;
        Port = port;
        Registration = registration;
    }
    public int GameId { get; set; }
    public string ServiceName { get; set; }
    public int Port { get; set; }
    public DateTimeOffset Registration { get; set; }

}
