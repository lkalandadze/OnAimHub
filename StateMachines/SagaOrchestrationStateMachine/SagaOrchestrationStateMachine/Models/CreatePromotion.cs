﻿namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommandDto? Promotion { get; set; }
    public List<CreateLeaderboardRecordCommand>? Leaderboards { get; set; }
    public List<GameConfigDto>? GameConfiguration { get; set; }
}