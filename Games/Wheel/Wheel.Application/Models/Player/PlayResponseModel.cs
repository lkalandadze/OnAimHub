#nullable disable

using GameLib.Domain.Abstractions;

namespace Wheel.Application.Models.Player;

public class PlayResponseModel
{
    public bool IsWin { get; set; }
    public int PrizeId { get; set; }
    public int? WheelIndex { get; set; }
    public decimal? Amount { get; set; }
    //public decimal TotalBalance { get; set; }
}