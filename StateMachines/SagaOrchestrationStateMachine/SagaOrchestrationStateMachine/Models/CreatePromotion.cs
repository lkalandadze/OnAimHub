namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommand Promotion { get; set; }
    public List<CreateLeaderboardRecordCommand>? Leaderboards { get; set; }
    public List<ConfigurationCreateModel>? GameConfiguration { get; set; }
}