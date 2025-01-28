namespace OnAim.Admin.APP.Services.FileServices;

public interface IFileManagementService
{
    Task<ApplicationResult<object>> UploadImage(UploadImageRequestModel file);
}
