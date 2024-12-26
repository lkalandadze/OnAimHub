namespace OnAim.Admin.Contracts.Dtos.Game;

public class PrizeDto
{
    public int Id { get; set; }
    public int Value { get; set; }
    public int Probability { get; set; }
    public int PrizeTypeId { get; set; }
    public int PrizeGroupId { get; set; }
    public string Name { get; set; }
    public int WheelIndex { get; set; }
}