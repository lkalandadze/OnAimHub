using SagaOrchestrationStateMachine.Controllers;

namespace SagaOrchestrationStateMachine.Models;

public class CreatePromotionDto
{
    public CreatePromotionCommandDto Promotion { get; set; }
    public List<CreateLeaderboardRecordCommand>? Leaderboards { get; set; }
    public List<GameConfigDto>? GameConfiguration { get; set; }
}
public class GameConfigDto
{
    public string GameName { get; set; }
    public GameConfiguration GameConfiguration { get; set; }
}

public class PrizeDto
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public decimal Probability { get; set; }
    public string CoinId { get; set; }
    public int PrizeGroupId { get; set; }
    public string Name { get; set; }
    public int WheelIndex { get; set; }
}

public class RoundDto
{
    public int Id { get; set; }
    public List<int> Sequence { get; set; }
    public int NextPrizeIndex { get; set; }
    public int ConfigurationId { get; set; }
    public List<PrizeDto> Prizes { get; set; }
    public string Name { get; set; }
}

public class GameConfigurationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public bool IsActive { get; set; }
    public int PromotionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string FromTemplateId { get; set; }
    public List<PriceDto> Prices { get; set; }
    public List<RoundDto> Rounds { get; set; }
}
public class PriceDto
{
    public string Id { get; set; }
    public decimal Value { get; set; }
    public decimal Multiplier { get; set; }
    public string CoinId { get; set; }
}
