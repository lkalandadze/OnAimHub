using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.Shared.DTOs.Segment
{
    public class SegmentDto
    {
        public string SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int PriorityLevel { get; set; }
        public int TotalActivePlayers { get; set; }
        public int TotalBlackListedPlayers { get; set; }
        public List<SegmentPlayerDto> ActivePlayers { get; set; }
        public List<SegmentPlayerDto> BlackListedPlayers { get; set; }
    }
}
