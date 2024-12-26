namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public int ConfigurationCount { get; set; }
    public List<int> PromotionIds { get; set; }
}
