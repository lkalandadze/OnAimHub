using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using System.Net.Http.Headers;
using System.Text;

namespace OnAim.Admin.APP.Services.FileServices;

public class FileManagementService : IFileManagementService
{
    public async Task<ApplicationResult<object>> UploadImage(UploadImageRequestModel file)
    {
        try
        {
            var imageClient = new ImageClient();
            var uploadResult = await imageClient.UploadImageAsync(file);

            var responseObject = JsonConvert.DeserializeObject<ApiResponse>(uploadResult);

            var imageUrl = responseObject?.Data?.Image;

            return new ApplicationResult<object> { Success = true, Data = imageUrl };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task<byte[]> GetFileContentAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
public class UploadImageRequestModel
{
    public IFormFile File { get; set; }
}

public class ImageClient
{
    private readonly HttpClient _httpClient;

    public ImageClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.88.138:5007/")
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> UploadImageAsync(UploadImageRequestModel file)
    {
        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(file.File.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "File", file.File.FileName);

            var response = await _httpClient.PostAsync("api/image/UploadImage", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public async Task<string> UploadImagesAsync(IFormFileCollection files)
    {
        using (var content = new MultipartFormDataContent())
        {
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent, "Files", file.FileName);
            }

            var response = await _httpClient.PostAsync("api/image/UploadImages", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
public class ApiResponse
{
    public bool Succeeded { get; set; }
    public object Errors { get; set; }
    public string Message { get; set; }
    public object ValidationErrors { get; set; }
    public ApiData Data { get; set; }
}

public class ApiData
{
    public string Image { get; set; }
}

public class HubClient
{
    private readonly HttpClient _httpClient;

    public HubClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.88.138:5002/")
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

