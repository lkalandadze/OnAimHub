namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UserActiveLeaderboards
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public string Error { get; set; }
    public object ValidationErrors { get; set; }
    public UserActiveLeaderboardData Data { get; set; }
}
