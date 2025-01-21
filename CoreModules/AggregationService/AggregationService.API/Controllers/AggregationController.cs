using AggregationService.Application.Models.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregationController : ControllerBase
{
    //private readonly HttpClient _client;
    //private readonly IDatabase _redis;
    private readonly IAggregationConfigurationService _aggregationConfigurationService;
    private readonly IConfigurationStore _configurationStore;

    public AggregationController(
                        /*HttpClient client, IConnectionMultiplexer muxer,*/
                        IAggregationConfigurationService aggregationConfigurationService,
                        IConfigurationStore configurationStore)
    {
        //_client = client;
        //_client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CachingApp", "1.0"));
        //_redis = muxer.GetDatabase();
        _aggregationConfigurationService = aggregationConfigurationService;
        _configurationStore = configurationStore;
    }

    [HttpPost(nameof(CreateConfigurations))]
    public async Task<IActionResult> CreateConfigurations([FromBody] List<CreateAggregationConfigurationModel> configs)
    {
        configs.ForEach(async config =>
        {
            await _aggregationConfigurationService.AddAggregationWithConfigurationsAsync(config);
        });

        return Ok(new { message = "Aggregation created successfully." });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAggregationConfigurationModel filter)
    {
        await _aggregationConfigurationService.AddAggregationWithConfigurationsAsync(filter);
        return Ok(new { message = "Aggregation created successfully." });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAggregationConfigurationModel filter)
    {
        await _aggregationConfigurationService.UpdateAggregationAsync(filter);
        return Ok(new { message = "Aggregation updated successfully." });
    }
    [HttpPost("Test")]
    public async Task<IActionResult> Test([FromBody] AggregationTriggerEvent test, CancellationToken cancellationToken)
    {
        await _aggregationConfigurationService.Test(test, cancellationToken);
        return Ok();
    }

}