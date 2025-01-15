#nullable disable

using Wheel.Application.Models.WheelPrize;

namespace Wheel.Application.Models.WheelPrizeGroup;

public class WheelPrizeGroupInitialData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<WheelPrizeInitialData> Prizes { get; set; }
}