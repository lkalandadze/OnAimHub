using AggregationService.Application.Models.Request;
using AggregationService.Application.Models.Response.AggregationConfigurations;
using AggregationService.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AggregationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregationController : ControllerBase
{
    //private readonly HttpClient _client;
    //private readonly IDatabase _redis;
    private readonly IAggregationConfigurationService _aggregationConfigurationService;
    public AggregationController(/*HttpClient client, IConnectionMultiplexer muxer,*/ IAggregationConfigurationService aggregationConfigurationService)
    {
        //_client = client;
        //_client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CachingApp", "1.0"));
        //_redis = muxer.GetDatabase();
        _aggregationConfigurationService = aggregationConfigurationService;
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

    //[HttpPost("process-play")]
    //public async Task<IActionResult> ProcessPlayRequest([FromBody] PlayRequest playRequest)
    //{
    //    var payloads = await _aggregationConfigurationService.ProcessPlayRequestAsync(playRequest);
    //    return Ok(payloads);
    //}

}