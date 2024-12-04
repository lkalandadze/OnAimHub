using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using System.Net.Http.Headers;

namespace OnAim.Admin.APP.Services.FileServices;

public class FileManagementService : IFileManagementService
{
    public async Task<ApplicationResult> UploadImage(UploadImageRequestModel file)
    {
        try
        {
            var imageClient = new ImageClient();
            var uploadResult = await imageClient.UploadImageAsync(file);

            var responseObject = JsonConvert.DeserializeObject<ApiResponse>(uploadResult);

            var imageUrl = responseObject?.Data?.Image;

            return new ApplicationResult { Success = true, Data = imageUrl };
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
            BaseAddress = new Uri("http://192.168.10.42:8006/")
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