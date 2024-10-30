namespace OnAim.Admin.Contracts.Dtos.Refer;

public class ReferralDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public DateTimeOffset InvitedDateTime { get; set; }
}