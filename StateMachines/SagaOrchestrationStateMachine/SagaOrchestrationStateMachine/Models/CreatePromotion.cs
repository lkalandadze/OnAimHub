namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public int? CreatedByUserId { get; set; }
    public CreatePromotionCommandDto Promotion { get; set; }
    public List<CreateLeaderboardRecord>? Leaderboards { get; set; }
    public List<GameConfigDto>? GameConfiguration { get; set; }
}