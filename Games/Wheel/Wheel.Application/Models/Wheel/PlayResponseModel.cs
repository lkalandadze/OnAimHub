#nullable disable

using Wheel;

namespace Wheel.Application.Models.Wheel;

public class PlayResponseModel
{
    public bool IsWin { get; set; }
    public int PrizeId { get; set; }
    public int? WheelIndex { get; set; }
    public decimal? WinAmount { get; set; }
}