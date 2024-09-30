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
        public List<PlayerDto> ActivePlayers { get; set; }
        public List<PlayerDto> BlackListedPlayers { get; set; }
        public List<ActsDto> Acts { get; set; }
        public List<ActsDto> History { get; set; }
    }
}
