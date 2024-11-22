namespace SagaOrchestrationStateMachine.Models;

public record CreatePromotionCommand(
    string Title,
    PromotionStatus Status,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    IEnumerable<string> SegmentIds,
    IEnumerable<CreatePromotionCoinModel> PromotionCoin);
public enum PromotionStatus
{
    NotStarted = 0,
    InProgress = 1,
    Finished = 2,
}
public class CreatePromotionCoinModel
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsTemplate { get; set; }
    public IEnumerable<CreateWithdrawOptionGroupModel> WithdrawOptionGroups { get; set; }
}
public class CreateWithdrawOptionGroupModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }
}
public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
    public bool IsTemplate { get; set; }
}
public enum EndpointContentType
{
    FromBody = 0,
    FromQuery = 1,
}
public enum CoinType
{
    Incomming = 0,
    Outgoing = 1,
    Internal = 2,
    Prize = 3,
}