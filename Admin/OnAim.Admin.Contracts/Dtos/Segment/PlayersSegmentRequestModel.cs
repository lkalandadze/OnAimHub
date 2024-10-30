using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Contracts.Dtos.Segment;

public class PlayersSegmentRequestModel
{
    [Required]
    public IFormFile File { get; set; }

    public int? ByUserId { get; set; }
}
