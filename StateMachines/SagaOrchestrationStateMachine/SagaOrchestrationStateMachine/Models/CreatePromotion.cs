using Hub.Application.Features.PromotionFeatures.Commands.CreatePromotion;
using SagaOrchestrationStateMachine.Controllers;

namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommandDto Promotion { get; set; }
    public List<CreateLeaderboardRecordCommand>? Leaderboards { get; set; }
    public List<GameConfiguration>? GameConfiguration { get; set; }
}