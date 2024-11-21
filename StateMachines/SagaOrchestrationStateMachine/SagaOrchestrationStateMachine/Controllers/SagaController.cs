using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using System.Text;
using System.Text.Json;

namespace SagaOrchestrationStateMachine.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SagaController> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    public SagaController(HttpClient httpClient, ILogger<SagaController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _retryPolicy = Policy
                   .Handle<HttpRequestException>()
                   .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                   .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    [HttpPost]
    public async Task OrchestratePromotionAsync(CreatePromotionCommand request)
    {
        try
        {
            var promotionResponse = await CreatePromotionAsync(request);
            _logger.LogInformation("Promotion created successfully: {PromotionId}", promotionResponse);


            //if (request.LeaderboardName != null)
            //{
            //    var leaderboardRequest = new CreateLeaderboardRequest
            //    {
            //        PromotionId = promotionResponse.PromotionId,
            //        LeaderboardName = request.LeaderboardName
            //    };

            //    var leaderboardResponse = await CreateLeaderboardAsync(leaderboardRequest);
            //    _logger.LogInformation("Leaderboard created successfully: {LeaderboardId}", leaderboardResponse.LeaderboardId);
            //}

            _logger.LogInformation("Saga completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Saga execution. Starting compensation.");

            await CompensateAsync(request);
        }
    }

    private async Task<IActionResult> CreatePromotionAsync(CreatePromotionCommand request)
    {
        try
        {
            var response = await _httpClient.PostAsync(
                "http://192.168.10.42:8003/HubApi/Admin/CreatePromotion",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<string>(content));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception("Failed to create promotion.", ex);
        }
    }

    private async Task CompensateAsync(CreatePromotionCommand request)
    {
        try
        {
            await _httpClient.DeleteAsync($"https://192.168.10.42:8003/hubapi/promotions/{request}");
            _logger.LogInformation("Promotion rollback successful for {PromotionId}", request);
            throw new Exception("Promotion Cannot save");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during compensation.");
        }
    }

}
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