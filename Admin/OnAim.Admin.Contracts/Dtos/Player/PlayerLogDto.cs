namespace OnAim.Admin.Contracts.Dtos.Player;

public class PlayerLogDto
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string Log { get; set; }
    public DateTimeOffset Date { get; set; }
    public string PlayerLogType { get; set; }
}
