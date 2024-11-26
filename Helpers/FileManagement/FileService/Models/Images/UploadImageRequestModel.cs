#nullable disable

namespace FileService.Models.Images;

public class UploadImageRequestModel
{
    public IFormFile File { get; set; }
}