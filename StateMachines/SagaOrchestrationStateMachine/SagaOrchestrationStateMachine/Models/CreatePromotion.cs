namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommand Promotion { get; set; }
    public List<CreateLeaderboardRecordCommand>? Leaderboards { get; set; }
    //public ConfigurationCreateModel? GameConfiguration { get; set; }
}