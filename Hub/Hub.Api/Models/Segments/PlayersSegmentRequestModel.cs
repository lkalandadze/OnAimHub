#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Hub.Api.Models.Segments;

public class PlayersSegmentRequestModel
{
    [Required]
    public IFormFile File { get; set; }

    public int? ByUserId { get; set; }
}