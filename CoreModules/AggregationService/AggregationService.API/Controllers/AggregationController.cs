using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net.Http.Headers;

namespace AggregationService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregationController : ControllerBase
{
    private readonly HttpClient _client;
    private readonly IDatabase _redis;
    public AggregationController(HttpClient client, IConnectionMultiplexer muxer)
    {
        _client = client;
        _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CachingApp", "1.0"));
        _redis = muxer.GetDatabase();
    }

}