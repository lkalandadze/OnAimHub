#nullable disable

using Wheel.Application.Models.WheelPrize;

namespace Wheel.Application.Models.Round;

public class RoundInitialData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<WheelPrizeInitialData> Prizes { get; set; }
}