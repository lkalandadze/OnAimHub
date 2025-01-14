#nullable disable

namespace PenaltyKicks.Application.Models.PenaltyKicks;

public class KickResponseModel
{
    public bool IsGoal { get; set; }
    public string GameState { get; set; }
    public int GoalsScored { get; set; }
    public int KicksRemaining { get; set; }
    public int RequiredGoalsToWin { get; set; }
}