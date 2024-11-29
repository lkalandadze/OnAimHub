namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommand Promotion { get; set; }
    public CreateLeaderboardRecordCommand? Leaderboard { get; set; }
    //public ConfigurationCreateModel? GameConfiguration { get; set; }
}