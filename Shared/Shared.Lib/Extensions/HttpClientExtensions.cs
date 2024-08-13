using System.Text.Json;

namespace Shared.Lib.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> GetAsync<TResponse>(this HttpClient httpClient, string host, string endpoint)
    {
        var url = host + endpoint;
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseString);
    }
}