using System.Text.Json;
using System.Text;

namespace Hub.Application.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> GetAsync<TResponse>(this HttpClient httpClient, string url, Dictionary<string, string>? queryParams = null)
    {
        if (queryParams != null && queryParams.Any())
        {
            var queryString = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            url = $"{url}?{queryString}";
        }

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseString);
    }
}