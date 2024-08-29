using GameLib.Domain.Abstractions;

namespace Wheel.Application.Models.Player;

public class PlayResultModel
{
    //public abstract SubGameTypes SubGameType { get; }
    public List<BasePrize> PrizeResults { get; set; }
    //public List<MissionsResultDto> MissionsResults { get; set; } = new();
    //public List<Suits> CompletedChanceSymbols { get; set; } = new();
    //public List<ChanceJackpotPrizeDto> WonChanceJackpotPrizes { get; set; } = new();
    internal long BetTransactionId { get; set; }
    public int Multiplier { get; set; }
}