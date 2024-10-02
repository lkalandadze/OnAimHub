using Microsoft.Extensions.Options;
using Polly.Timeout;
using Polly.Wrap;
using Polly;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using OnAim.Admin.Domain.Exceptions;
using System.Net.Http.Headers;

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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJQbGF5ZXJJZCI6IjI3IiwiVXNlck5hbWUiOiIxOTEzIiwiU2VnbWVudElkcyI6ImRlZmF1bHQiLCJleHAiOjIzMjc3ODY0ODUsImlzcyI6IkhVQiIsImF1ZCI6IkhVQi1BVURJRU5DRSJ9.wXw5FHhnnr_a1CM_QicKNlQqKf7_Y6WlHUHAJeL8GVCKDL8JFLn7Tp9NboDuXs7ztoZPritBVOixSgXNQ8J3Iw");

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

        var content = new StringContent(
            Serialize(obj),
            Encoding.UTF8,
            "application/json"
        );

        var res = await _httpClient.PostAsync(uri, content, ct);

        if (!res.IsSuccessStatusCode)
        {
            var errorContent = await res.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Request failed with status code {res.StatusCode}. Response content: {errorContent}");
        }

        var responseContent = await res.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(responseContent))
            throw new Exception("The response is empty or not in valid JSON format");

        try
        {
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
        catch (JsonReaderException ex)
        {
            throw new Exception($"Failed to deserialize JSON response: {responseContent}", ex);
        }
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

    public async Task<HttpResponseMessage> Delete(
            string uri,
            CancellationToken ct = default
        )
    {
        var res = await _httpClient.DeleteAsync(
            uri,
            ct
        );

        if (!res.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(res);

        return res;
    }

    public async Task<HttpResponseMessage> PostMultipartAsync(string uri, MultipartFormDataContent content, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsync(uri, content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Request failed with status code {response.StatusCode}. Response content: {errorContent}");
        }

        return response;
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
