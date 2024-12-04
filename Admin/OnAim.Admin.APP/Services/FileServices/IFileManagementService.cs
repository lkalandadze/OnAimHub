using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.FileServices;

public interface IFileManagementService
{
    Task<ApplicationResult> UploadImage(UploadImageRequestModel file);
}
