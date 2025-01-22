using OnAim.Admin.Contracts.Dtos.Refer;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Dtos.Transaction;

namespace OnAim.Admin.Contracts.Dtos.Player;

public class PlayerDto
{
    public int Id { get; set; }
    public string? PlayerName { get; set; }
    public bool IsBanned { get; set; }
    public List<SegmentDto>? Segments { get; set; }
    //public List<TransactionDto>? Transactions { get; set; }
    public DateTimeOffset? RegistrationDate { get; set; }
    public DateTimeOffset? LastVisit { get; set; }
    public RefereeDto? Referee { get; set; }
    public List<PlayerBalanceDto>? PlayerBalances { get; set; }
    //public List<PlayerLogDto>? PlayerLogs { get; set; }
    public List<ReferralDto>? Referrals { get; set; }
}
