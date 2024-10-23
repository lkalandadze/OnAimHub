using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.Shared.DTOs.Game;

public class GameListDto
{
    public List<GameListDtoItem> Data { get; set; }
}

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
