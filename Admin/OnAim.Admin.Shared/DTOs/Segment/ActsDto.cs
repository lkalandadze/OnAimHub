using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Shared.DTOs.Segment;

public class ActsDto
{
    public int Id { get; set; }
    public string Note { get; set; }
    public int? UploadedBy { get; set; }
    public DateTime? UploadedOn { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }
}
public class PlayersSegmentRequestModel
{
    [Required]
    public IFormFile File { get; set; }

    public int? ByUserId { get; set; }
}
