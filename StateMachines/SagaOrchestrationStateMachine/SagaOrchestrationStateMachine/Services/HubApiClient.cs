using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Polly.Timeout;
using Polly.Wrap;
using System.Text;
using Nito.AsyncEx;
using Polly;

namespace SagaOrchestrationStateMachine.Services;

public class HubApiClient : IHubApiClient
{
    private readonly HttpClient _httpClient;
    private readonly HubApiClientOptions _options;
    private readonly AsyncPolicyWrap<HttpResponseMessage> _combinedPolicy;
    private readonly string _username;
    private readonly string _password;

    public HubApiClient(
        HttpClient httpClient,
        IOptions<HubApiClientOptions> options,
        IOptions<PolicyOptions> policyOptions,
        string username,
        string password)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _username = username;
        _password = password;

        var byteArray = Encoding.ASCII.GetBytes($"admin:password");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var retryPolicy = Policy
       .Handle<HttpRequestException>()
       .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
       .RetryAsync(policyOptions.Value.RetryCount);

        var timeoutPolicy = Policy.TimeoutAsync(policyOptions.Value.TimeOutDuration, TimeoutStrategy.Pessimistic);

        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(3, 6);

        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                policyOptions.Value.RetryCount + 1,
                System.TimeSpan.FromSeconds(policyOptions.Value.BreakDuration)
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

    public async Task<HttpResponseMessage> Delete(string uri, object obj, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(uri, UriKind.RelativeOrAbsolute),
            Content = obj != null
                   ? new StringContent(
                         Newtonsoft.Json.JsonConvert.SerializeObject(obj),
                         Encoding.UTF8,
                         "application/json")
                   : null
        };

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
            throw new HubAPIRequestFailedException(response);

        return response;
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
public interface IHubApiClient
{
    Task<T> Get<T>(
   string uri,
   CancellationToken ct = default
    );
    Task<Stream> GetAsStream(
        string uri,
        CancellationToken ct = default
    );
    Task<HttpResponseMessage> PostAsJson(
        string uri,
        object obj,
        CancellationToken ct = default
    );
    Task<HttpResponseMessage> PutAsJson(
        string uri,
        object obj,
        CancellationToken ct = default
    );
    Task<T> PostAsJsonAndSerializeResultTo<T>(
        string uri,
        object obj,
        CancellationToken ct = default
    );

    Task<HttpResponseMessage> Delete(
        string uri, object obj, CancellationToken ct = default
    );

    Task<HttpResponseMessage> PostMultipartAsync(string uri, MultipartFormDataContent content, CancellationToken ct = default);

}
public class HubAPIRequestFailedException : Exception
{
    public HubAPIRequestFailedException(HttpResponseMessage res) :
    base(AsyncContext.Run(async () => (await res.Content.ReadFromJsonAsync<IEnumerable<HubAPIErrorResponseDTO>>()).FirstOrDefault().Message))
    { }

    public HubAPIRequestFailedException(string msg) : base(msg) { }

}
public sealed record HubAPIErrorResponseDTO(string Message);
public class HubApiClientOptions
{
    public string BaseApiAddress { get; set; } = default!;
    public string Endpoint { get; set; } = default!;
}
public class LeaderBoardApiClientOptions
{
    public string BaseApiAddress { get; set; } = default!;
    public string Endpoint { get; set; } = default!;
}
public class PolicyOptions : ICircuitBreakerPolicyOptions, IRetryPolicyOptions, ITimeoutPolicyOptions
{
    public int RetryCount { get; set; } = 3;
    public int BreakDuration { get; set; } = 30;
    public int TimeOutDuration { get; set; } = 15;
}
public interface ICircuitBreakerPolicyOptions
{
    int RetryCount { get; set; }
    int BreakDuration { get; set; }
}
public interface IRetryPolicyOptions
{
    int RetryCount { get; set; }
}
public interface ITimeoutPolicyOptions
{
    public int TimeOutDuration { get; set; }
}