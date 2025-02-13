﻿using Hub.Application.Features.PromotionFeatures.Commands.Delete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using SagaOrchestrationStateMachine.Models;
using SagaOrchestrationStateMachine.Services;

namespace SagaOrchestrationStateMachine.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SagaController> _logger;
    private readonly IHubApiClient _hubApiClient;
    private readonly IWheelApiClientApiClient _wheelApiClientApiClient;
    private readonly ILeaderboardApiClientApiClient _leaderboardApiClientApiClient;
    private readonly LeaderBoardApiClientOptions _leaderBoardApiClientOptions;
    private readonly WheelApiClientOptions _wheelApiClientOptions;
    private readonly HubApiClientOptions _options;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    public SagaController(
        HttpClient httpClient,
        ILogger<SagaController> logger,
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        IWheelApiClientApiClient wheelApiClientApiClient,
        IOptions<WheelApiClientOptions> wheelApiClientOptions,
        ILeaderboardApiClientApiClient leaderboardApiClientApiClient,
        IOptions<LeaderBoardApiClientOptions> leaderBoardApiClientOptions
        )
    {
        _httpClient = httpClient;
        _logger = logger;
        _hubApiClient = hubApiClient;
        _wheelApiClientApiClient = wheelApiClientApiClient;
        _leaderboardApiClientApiClient = leaderboardApiClientApiClient;
        _leaderBoardApiClientOptions = leaderBoardApiClientOptions.Value;
        _wheelApiClientOptions = wheelApiClientOptions.Value;
        _options = options.Value;
        _retryPolicy = Policy
                   .Handle<HttpRequestException>()
                   .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                   .WaitAndRetryAsync(3, retryAttempt => System.TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    [HttpPost]
    public async Task<IActionResult> OrchestratePromotionAsync(CreatePromotionDto request)
    {
        var correlationId = Guid.NewGuid();
        try
        {
            request.Promotion.CorrelationId = correlationId;
            int promotionId;
            List<PromotionResponseCoin> coins;

            try
            {
                var res = await CreatePromotionAsync(request.Promotion);
                promotionId = res.PromotionId;
                coins = res.Coins;
                _logger.LogInformation("Promotion created successfully: {PromotionId}", promotionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion.");
                await CompensateAsync(correlationId);
                return BadRequest(new { Success = false, Message = "Failed to create promotion.", Error = ex.Message });
            }

            if (request.Leaderboards.Count != 0)
            {
                try
                {
                    foreach (var leaderboard in request.Leaderboards)
                    {
                        leaderboard.CorrelationId = correlationId;
                        leaderboard.PromotionId = promotionId;

                        if (leaderboard != null)
                        {
                            try
                            {
                                var command = new CreateLeaderboardRecordCommand
                                {
                                    AnnouncementDate = leaderboard.AnnouncementDate,
                                    CorrelationId = correlationId,
                                    Description = leaderboard.Description,
                                    EndDate = leaderboard.EndDate,
                                    EventType = leaderboard.EventType,
                                    IsGenerated = leaderboard.IsGenerated,
                                    LeaderboardPrizes = leaderboard.LeaderboardPrizes.Select(x => new CreateLeaderboardRecordPrizeCommandItem
                                    {
                                        CoinId = $"{promotionId}_{x.CoinId}",
                                        Amount = x.Amount,
                                        EndRank = x.EndRank,
                                        StartRank = x.StartRank,
                                    }).ToList(),
                                    PromotionId = promotionId,
                                    PromotionName = leaderboard.PromotionName,
                                    RepeatType = leaderboard.RepeatType,
                                    RepeatValue = leaderboard.RepeatValue,
                                    ScheduleId = leaderboard.ScheduleId,
                                    StartDate = leaderboard.StartDate,
                                    Status = leaderboard.Status,
                                    TemplateId = leaderboard.TemplateId,
                                    Title = leaderboard.Title,
                                    CreatedBy = leaderboard.CreatedBy,
                                };

                                var leaderboardResponse = await CreateLeaderboardRecordAsync(command);
                                _logger.LogInformation("LeaderboardRecord created successfully: {LeaderboardRecord}", leaderboardResponse);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error creating leaderboard record.");
                                await CompensateAsync(correlationId);
                                return BadRequest(new { Success = false, Message = "Failed to create leaderboard.", Error = ex.Message });
                            }
                        }

                        _logger.LogInformation("Leaderboard created successfully: {Leaderboard}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing leaderboards.");
                    await CompensateAsync(correlationId);
                    return BadRequest(new { Success = false, Message = "Failed to process leaderboards.", Error = ex.Message });
                }
            }

            if (request.GameConfiguration.Count != 0)
            {
                try
                {
                    foreach (var config in request.GameConfiguration)
                    {
                        //config.GameConfiguration.CorrelationId = correlationId;
                        //config.GameConfiguration.PromotionId = promotionId;                    

                        if (config != null)
                        {
                            try
                            {
                                var gameResponse = await CreateGameConfiguration(config.GameConfiguration);
                                _logger.LogInformation("Game configuration created successfully: {ConfigId}", gameResponse);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error creating game configuration.");
                                await CompensateAsync(correlationId);
                                return BadRequest(new { Success = false, Message = "Failed to create game configuration.", Error = ex.Message });
                            }
                        }

                        _logger.LogInformation("Game configuration created successfully: {Configuration}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing game configurations.");
                    await CompensateAsync(correlationId);
                    return BadRequest(new { Success = false, Message = "Failed to process game configurations.", Error = ex.Message });
                }
            }

            _logger.LogInformation("Saga completed successfully.");
            return Ok(new { Success = true, CorrelationId = correlationId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Saga execution.");
            await CompensateAsync(correlationId);
            return StatusCode(500, new { Success = false, Message = "Internal server error during saga execution.", Error = ex.Message });
        }
    }

    private async Task<PromotionResponse> CreatePromotionAsync(CreatePromotionCommandDto request)
    {
        try
        {
            var promotionId = await _hubApiClient.PostAsJsonAndSerializeResultTo<PromotionResponse>($"{_options.Endpoint}Admin/CreatePromotion", request);
            return promotionId;
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
            await _leaderboardApiClientApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}CreateLeaderboardRecord", leaderboard);
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task<IActionResult> CreateGameConfiguration(object model)
    {
        try
        {
            await _wheelApiClientApiClient.PostAsJson($"{_wheelApiClientOptions.Endpoint}Admin/CreateConfiguration", model);
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
            var req = new DeletePromotionCommand(request);
            var lead = new DeleteLeaderboardRecordCommand(request);
            await _hubApiClient.Delete($"{_options.Endpoint}Admin/DeletePromotion", req);
            await _leaderboardApiClientApiClient.PostAsJson($"{_leaderBoardApiClientOptions.Endpoint}DeleteLeaderboardRecord", lead);
            //await _wheelApiClientApiClient.Delete($"{_leaderBoardApiClientOptions.Endpoint}DeleteLeaderboardRecord", request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create promotion.");
            throw new Exception(ex.Message, ex);
        }
    }
}