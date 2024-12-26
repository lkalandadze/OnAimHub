namespace OnAim.Admin.Contracts.Dtos.Game;

public class ResponseData
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public object Error { get; set; }
    public object ValidationErrors { get; set; }
    public List<GameData> Data { get; set; }
}