using FileService.Configurations;
using FileService.Models.Images;
using FileService.Services.Abstract;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Wrappers;

namespace FileService.Services.Concrete;

public class ImageService : IImageService
{
    private readonly FileServiceConfiguration _fileServiceConfig;
    private readonly ImageStorageConfiguration _imageStorageConfig;

    public ImageService(IOptions<FileServiceConfiguration> fileServiceConfig, IOptions<ImageStorageConfiguration> imageStorageConfig)
    {
        _fileServiceConfig = fileServiceConfig.Value;
        _imageStorageConfig = imageStorageConfig.Value;
    }

    public async Task<Response<ImageResponseModel>> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "File is null or empty.");
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_imageStorageConfig.AllowedExtensions.Contains(fileExtension))
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "Invalid file type.");
        }

        string sanitizedFileName = Path.GetFileName(file.FileName);
        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var localPath = new Uri(_fileServiceConfig.Host).LocalPath;
        var uploadPath = Path.Combine(localPath, _imageStorageConfig.Directory);

        try
        {
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await file.CopyToAsync(stream);
            }

            return new Response<ImageResponseModel>()
            {
                Succeeded = true,
                Data = new ImageResponseModel { Image = $"{_fileServiceConfig.Host}/{_imageStorageConfig.Directory}/{uniqueFileName}" },
            };
        }
        catch (Exception)
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "An error occurred while uploading the file.");
        }
    }

    public async Task<Response<ImagesResponseModel>> UploadImagesAsync(IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "No files to upload.");
        }

        var imageUploadTasks = files.Select(file => UploadImageAsync(file));
        var responses = await Task.WhenAll(imageUploadTasks);

        var uploadedImages = responses.Where(response => response.Data != null && string.IsNullOrEmpty(response.Data.Image))
                                      .Select(response => response.Data!.Image);

        return new Response<ImagesResponseModel>
        {
            Succeeded = true,
            Data = new ImagesResponseModel { Images = uploadedImages }
        };
    }
}