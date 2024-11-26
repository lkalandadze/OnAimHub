#nullable disable

namespace FileService.Configurations;

public class ImageStorageConfiguration
{
    public string Directory { get; set; }
    public string[] AllowedExtensions { get; set; }
}