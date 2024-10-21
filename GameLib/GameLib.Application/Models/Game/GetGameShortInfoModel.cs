#nullable disable

namespace GameLib.Application.Models.Game;

public class GetGameShortInfoModel
{
    public bool Status { get; set; }
    public string Description { get; set; }
    public int ConfigurationCount { get; set; }
    public IEnumerable<string> Segments { get; set; }
}