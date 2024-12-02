using OnAim.Admin.APP.Services.FileServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IFileManagementService
{
    Task<ApplicationResult> UploadImage(UploadImageRequestModel file);
}
