namespace OnAim.Admin.Contracts.Dtos.Segment;

public class ActsHistoryDto
{
    public int Id { get; set; }
    public int? UploadedBy { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public string Note { get; set; }
    public DateTimeOffset? UploadedOn { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }
}
