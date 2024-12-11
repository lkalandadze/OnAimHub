namespace OnAim.Admin.Contracts.Dtos.Game;

public class CreatePrizeDto
{
    public int Value { get; set; }
    public int Probability { get; set; }
    public int PrizeTypeId { get; set; }
    public int PrizeGroupId { get; set; }
    public string Name { get; set; }
    public int WheelIndex { get; set; }
}