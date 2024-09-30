using Microsoft.Extensions.Options;
using Polly.Timeout;
using Polly.Wrap;
using Polly;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using OnAim.Admin.Domain.Exceptions;

namespace OnAim.Admin.APP.Shared.Clients;

public class HubApiClient : IHubApiClient
{
    private readonly HttpClient _httpClient;
    private readonly HubApiClientOptions _options;
    private readonly AsyncPolicyWrap<HttpResponseMessage> _combinedPolicy;

    public HubApiClient(HttpClient httpClient, IOptions<HubApiClientOptions> options, IOptions<PolicyOptions> policyOptions)
    {
        _httpClient = httpClient.NotBeNull();
        _options = options.Value;

        var retryPolicy = Polly.Policy
       .Handle<HttpRequestException>()
       .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
       .RetryAsync(policyOptions.Value.RetryCount);

        var timeoutPolicy = Polly.Policy.TimeoutAsync(policyOptions.Value.TimeOutDuration, TimeoutStrategy.Pessimistic);

        var bulkheadPolicy = Polly.Policy.BulkheadAsync<HttpResponseMessage>(3, 6);

        var circuitBreakerPolicy = Polly.Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                policyOptions.Value.RetryCount + 1,
                TimeSpan.FromSeconds(policyOptions.Value.BreakDuration)
            );

        var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, bulkheadPolicy);

        _combinedPolicy = combinedPolicy.WrapAsync(timeoutPolicy);
    }

    public async Task<T> Get<T>(string uri, CancellationToken ct = default)
    {
        var res = await _httpClient.GetAsync(
           uri,
           ct
       );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());

    }

    public async Task<Stream> GetAsStream(string uri, CancellationToken ct = default)
    {
        var res = await _httpClient.GetAsync(
           uri,
           ct
       );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return await res.Content.ReadAsStreamAsync();

    }

    public async Task<HttpResponseMessage> PostAsJson(string uri, object obj, CancellationToken ct = default)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        var res = await _httpClient.PostAsync(
            uri,
            new StringContent(
                Serialize(obj),
                Encoding.UTF8,
                "application/json"
            ),
            ct
        );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return res;

    }

    public async Task<T> PostAsJsonAndSerializeResultTo<T>(string uri, object obj, CancellationToken ct = default)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        var res = await _httpClient.PostAsync(
            uri,
            new StringContent(
                Serialize(obj),
                Encoding.UTF8,
                "application/json"
            ),
            ct
        );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());

    }

    public async Task<HttpResponseMessage> PutAsJson(string uri, object obj, CancellationToken ct = default)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        var res = await _httpClient.PutAsync(
            uri,
            new StringContent(
                Serialize(obj),
                Encoding.UTF8,
                "application/json"
            ),
            ct
        );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return res;

    }

    private string Serialize(object obj)
        => JsonConvert.SerializeObject(
            obj,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        );

}
