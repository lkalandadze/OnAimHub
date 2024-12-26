using AggregationService.Application.Models.Aggregations;
using AggregationService.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AggregationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregationController : ControllerBase
{
    //private readonly HttpClient _client;
    //private readonly IDatabase _redis;
    private readonly IAggregationService _aggregationService;
    public AggregationController(/*HttpClient client, IConnectionMultiplexer muxer,*/ IAggregationService aggregationService)
    {
        //_client = client;
        //_client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CachingApp", "1.0"));
        //_redis = muxer.GetDatabase();
        _aggregationService = aggregationService;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAggregationModel filter)
    {
        await _aggregationService.AddAggregationWithConfigurationsAsync(filter);
        return Ok(new { message = "Aggregation created successfully." });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAggregationModel filter)
    {
        await _aggregationService.UpdateAggregationAsync(filter);
        return Ok(new { message = "Aggregation updated successfully." });
    }

}