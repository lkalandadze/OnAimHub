using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Services.FileServices;

namespace OnAim.Admin.API.Controllers;

public class FileController : ApiControllerBase
{
    private readonly IFileManagementService _fileManagementService;

    public FileController(IFileManagementService fileManagementService)
    {
        _fileManagementService = fileManagementService;
    }

    [HttpPost(nameof(UploadImage))]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestModel request)
    {
        try
        {

            var response = await _fileManagementService.UploadImage(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    private async Task<Stream> GetFileContentStreamAsync(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}