using System.Text;
using System.Text.Json;

namespace Shared.Lib.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> CustomGetAsync<TResponse>(this HttpClient httpClient, string host, string endpoint)
    {
        var url = host + endpoint;
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseString);
    }

    public static async Task<TResponse?> CustomPostAsync<TResponse>(this HttpClient httpClient, string host, string endpoint, object? content = null)
    {
        var url = host + endpoint;
        var jsonContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, jsonContent);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}