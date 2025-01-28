using OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Contracts.Dtos.AuditLog;

namespace OnAim.Admin.APP.Services.AdminServices.User;

public interface IUserService
{
    Task<ApplicationResult<string>> ActivateAccount(string email, string code);
    Task<ApplicationResult<bool>> ChangePassword(string email, string oldPassword, string newPassword);
    Task<ApplicationResult<bool>> Create(string email, string firstName, string lastName, string phone);
    Task<ApplicationResult<bool>> Delete(List<int> userIds);
    Task<ApplicationResult<bool>> ForgotPassword(string email);
    Task<ApplicationResult<bool>> ResetPassword(string email, string code, string password);
    Task<AuthResultDto> Login(LoginUserRequest model);
    Task<ApplicationResult<bool>> ProfileUpdate(int id, ProfileUpdateRequest profileUpdateRequest);
    Task<ApplicationResult<bool>> Registration(string email, string password, string firstName, string lastName, string phone, DateTime DateOfBirth);
    Task<AuthResultDto> TwoFA(string email, string otpCode);
    Task<ApplicationResult<string>> Update(int id, UpdateUserRequest model);

    Task<ApplicationResult<PaginatedResult<UsersModel>>> GetAll(UserFilter filter);
    Task<ApplicationResult<GetUserModel>> GetById(int id);
    Task<ApplicationResult<PaginatedResult<AuditLogDto>>> GetUserLogs(int id, AuditLogFilter filter);
    Task<ApplicationResult<OnAim.Admin.Domain.Entities.User>> GetByEmail(string email);
}