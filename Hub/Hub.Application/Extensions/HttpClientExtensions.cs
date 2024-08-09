using System.Text.Json;
using System.Text;

namespace Hub.Application.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> GetAsync<TResponse>(this HttpClient httpClient, string url, params object[] urlParams)
    {
        var formattedUrl = string.Format(url, urlParams);
        var response = await httpClient.GetAsync(formattedUrl);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseString);
    }
}