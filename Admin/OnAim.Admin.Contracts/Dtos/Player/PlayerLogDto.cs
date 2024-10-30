namespace OnAim.Admin.Contracts.Dtos.Player;

public class PlayerLogDto
{
    public int Id { get; set; }
    public string Log { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string PlayerLogType { get; set; }
}
