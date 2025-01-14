#nullable disable

using PenaltyKicks.Domain.Entities;

namespace PenaltyKicks.Application.Models.PenaltyKicks;

public class BetResponseModel
{
    public int PrizeId { get; set; }
    public int PrizeValue { get; set; }
    public string Coin { get; set; }
}