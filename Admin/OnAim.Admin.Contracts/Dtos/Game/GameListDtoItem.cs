namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameListDtoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Configurations { get; set; }
    public string Description { get; set; }
    public List<string> Segments { get; set; }
    public DateTime LaunchDate { get; set; }
    public string Status { get; set; }
}
