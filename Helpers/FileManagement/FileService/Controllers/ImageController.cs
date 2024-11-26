using FileService.Models.Images;
using FileService.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost(nameof(UploadImage))]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestModel request)
    {
        var response = await _imageService.UploadImageAsync(request.File);

        return Ok(response);
    }

    [HttpPost(nameof(UploadImages))]
    public async Task<IActionResult> UploadImages([FromForm] UploadImagesRequestModel request)
    {
        var response = await _imageService.UploadImagesAsync(request.Files);

        return Ok(new { Urls = response });
    }
}