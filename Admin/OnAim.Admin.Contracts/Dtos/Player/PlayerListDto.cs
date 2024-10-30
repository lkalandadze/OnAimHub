namespace OnAim.Admin.Contracts.Dtos.Player;

public class PlayerListDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public DateTimeOffset? RegistrationDate { get; set; }
    public DateTimeOffset? LastVisit { get; set; }
    public string? Segment { get; set; }
    public bool? IsBanned { get; set; }

}
