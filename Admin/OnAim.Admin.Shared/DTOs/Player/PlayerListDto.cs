namespace OnAim.Admin.Shared.DTOs.Player;

public class PlayerListDto
{
    public int PlayerId { get; set; }
    public string? PlayerName { get; set; }
    public DateTimeOffset? RegistrationDate { get; set; }
    public DateTimeOffset? LastVisit { get; set; }
    public string? Segment {  get; set; }
    public string? Status { get; set; }

}
