#nullable disable

namespace Shared.Application;

public class AuthorizedPlayer
{
    public int PlayerId { get; set; }
    public string UserName { get; set; }
    public int SegmentId { get; set; }
}