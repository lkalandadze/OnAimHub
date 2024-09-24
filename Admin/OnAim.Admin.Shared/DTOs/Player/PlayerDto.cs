using OnAim.Admin.Shared.DTOs.Player.Balance;
using OnAim.Admin.Shared.DTOs.Player.Log;
using OnAim.Admin.Shared.DTOs.Refer;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.DTOs.Transaction;

namespace OnAim.Admin.Shared.DTOs.Player
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string? PlayerName { get; set; }
        public List<SegmentDto>? Segments { get; set; }
        public List<TransactionDto>? Transactions { get; set; }
        public DateTimeOffset? RegistrationDate { get; set; }
        public DateTimeOffset? LastVisit { get; set; }
        public RefereeDto? Referee { get; set; }
        public List<PlayerBalanceDto>? PlayerBalances { get; set; }
        public List<PlayerLogDto>? PlayerLogs { get; set; }
        public List<ReferralDto>? Referrals { get; set; }
    }
}
