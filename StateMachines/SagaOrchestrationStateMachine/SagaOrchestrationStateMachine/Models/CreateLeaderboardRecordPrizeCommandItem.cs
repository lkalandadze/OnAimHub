namespace SagaOrchestrationStateMachine.Models;

public class CreateLeaderboardRecordPrizeCommandItem
{
    public int StartRank { get; set; }
    public int EndRank { get; set; }
    public string Coin { get; set; }
    public int Amount { get; set; }
}