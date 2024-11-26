#nullable disable

namespace FileService.Models.Images;

public class UploadImagesRequestModel
{
    public IEnumerable<IFormFile> Files { get; set; }
}