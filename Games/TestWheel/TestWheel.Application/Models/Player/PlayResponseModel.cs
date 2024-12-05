#nullable disable

using GameLib.Domain.Abstractions;

namespace TestWheel.Application.Models.Player;

public class PlayResponseModel
{
    public List<BasePrize> PrizeResults { get; set; }
    internal long BetTransactionId { get; set; }
    public int Multiplier { get; set; }
}