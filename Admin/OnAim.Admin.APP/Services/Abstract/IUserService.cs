using OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IUserService
{
    Task<ApplicationResult> ActivateAccount(string email, string code);
    Task<ApplicationResult> ChangePassword(string email, string oldPassword, string newPassword);
    Task<ApplicationResult> Create(string email, string firstName, string lastName, string phone);
    Task<ApplicationResult> Delete(List<int> userIds);
    Task<ApplicationResult> ForgotPassword(string email);
    Task<ApplicationResult> ResetPassword(string email, string code, string password);
    Task<AuthResultDto> Login(LoginUserRequest model);
    Task<ApplicationResult> ProfileUpdate(int id, ProfileUpdateRequest profileUpdateRequest);
    Task<ApplicationResult> Registration(string email, string password, string firstName, string lastName, string phone, DateTime DateOfBirth);
    Task<AuthResultDto> TwoFA(string email, string otpCode);
    Task<ApplicationResult> Update(int id, UpdateUserRequest model);

    Task<ApplicationResult> GetAll(UserFilter filter);
    Task<ApplicationResult> GetById(int id);
    Task<ApplicationResult> GetUserLogs(int id, AuditLogFilter filter);
    Task<ApplicationResult> GetByEmail(string email);
}