using FileService.Models.Images;
using Shared.Lib.Wrappers;

namespace FileService.Services.Abstract;

public interface IImageService
{
    Task<Response<ImageResponseModel>> UploadImageAsync(IFormFile file);

    Task<Response<ImagesResponseModel>> UploadImagesAsync(IEnumerable<IFormFile> files);
}