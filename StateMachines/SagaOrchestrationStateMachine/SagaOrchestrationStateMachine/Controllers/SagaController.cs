using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using SagaOrchestrationStateMachine.Models;

namespace SagaOrchestrationStateMachine.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SagaController> _logger;
    private readonly LeaderboardService _leaderboardService;
    private readonly HubService _hubService;
    private readonly WheelService _wheelService;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    public SagaController(
        HttpClient httpClient, 
        ILogger<SagaController> logger, 
        LeaderboardService leaderboardService,
        HubService hubService,
        WheelService wheelService
        )
    {
        _httpClient = httpClient;
        _logger = logger;
        _leaderboardService = leaderboardService;
        _hubService = hubService;
        _wheelService = wheelService;
        _retryPolicy = Policy
                   .Handle<HttpRequestException>()
                   .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                   .WaitAndRetryAsync(3, retryAttempt => System.TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    [HttpPost]
    public async Task OrchestratePromotionAsync(CreatePromotionDto request)
    {
        var correlationId = Guid.NewGuid();
        try
        {
            request.Promotion.CorrelationId = correlationId;

            // Step 1: Create Promotion
            try
            {
                var promotionResponse = await CreatePromotionAsync(request.Promotion);
                _logger.LogInformation("Promotion created successfully: {PromotionId}", promotionResponse);
            }
            catch
            {
                await CompensateAsync(correlationId);
                throw;
            }

            // Step 2: Create Leaderboard
            if (request.Leaderboard != null)
            {
                try
                {
                    request.Leaderboard.LeaderboardRecord.CorrelationId = correlationId;
                    if (request.Leaderboard.LeaderboardTemplate != null)
                    {
                        request.Leaderboard.LeaderboardTemplate.CorrelationId = correlationId;
                    }

                    if (request.Leaderboard.LeaderboardRecord != null)
                    {
                        try
                        {
                            var leaderboardResponse = await CreateLeaderboardRecordAsync(request.Leaderboard.LeaderboardRecord);
                            _logger.LogInformation("LeaderboardRecord created successfully: {LeaderboardRecord}", leaderboardResponse);
                        }
                        catch
                        {
                            await CompensateAsync(correlationId);
                            throw;
                        }
                    }

                    if (request.Leaderboard.LeaderboardTemplate != null)
                    {
                        try
                        {
                            var leaderboardTemplateResponse = await CreateLeaderboardTemplateAsync(request.Leaderboard.LeaderboardTemplate);
                            _logger.LogInformation("LeaderboardTemplate created successfully: {LeaderboardTemplate}", leaderboardTemplateResponse);
                        }
                        catch
                        {
                            await CompensateAsync(correlationId);
                            throw;
                        }
                    }

                    _logger.LogInformation("Leaderboard created successfully: {Leaderboard}");
                }
                catch
                {
                    await CompensateAsync(correlationId);
                    throw;
                }
            }

            // Step 3: Create Game Configuration
            if (request.GameConfiguration.ConfigurationJson != null)
            {
                try
                {
                    var gameResponse = await CreateGameConfiguration(request.GameConfiguration);
                    _logger.LogInformation("Configuration created successfully: {ConfigId}", gameResponse);
                }
                catch
                {
                    await CompensateAsync(correlationId);
                    throw;
                }
            }

            _logger.LogInformation("Saga completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Saga execution.");
            await CompensateAsync(correlationId);
        }
    }

    private async Task<IActionResult> CreatePromotionAsync(CreatePromotionCommand request)
    {
        try
        {
            await _hubService.CreatePromotionAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception(ex.Message, ex);
        }
    }
    private async Task<IActionResult> CreateLeaderboardRecordAsync(CreateLeaderboardRecordCommand leaderboard)
    {
        try
        {
            await _leaderboardService.CreateLeaderboardRecordAsync(leaderboard);
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    private async Task<IActionResult> CreateLeaderboardTemplateAsync(CreateLeaderboardTemplateCommand leaderboard)
    {
        try
        {
            await _leaderboardService.CreateLeaderboardTemplateAsync(leaderboard);
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    private async Task<IActionResult> CreateGameConfiguration(ConfigurationCreateModel model)
    {
        try
        {
            await _wheelService.CreateConfigurationAsync(model);
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    private async Task CompensateAsync(Guid request)
    {
        try
        {
            var req = new DeletePromotionCommand();
            var lead = new DeleteLeaderboardRecordCommand();
            var temp = new DeleteLeaderboardTemplateCommand();
            lead.CorrelationId = request;
            temp.CorrelationId = request;
            req.CorrelationId = request;
            await _hubService.DeletePromotionAsync(req);
            await _leaderboardService.DeleteLeaderboardRecordAsync(lead);
            await _leaderboardService.DeleteLeaderboardTemplateAsync(temp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception(ex.Message, ex);
        }
    }
}