using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Create;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Registration;
using OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;
using OnAim.Admin.Shared.DTOs.AuditLog;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Helpers.Password;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Paging;
using System.Linq.Expressions;
using System.Security.Claims;

namespace OnAim.Admin.APP.Services.User;

public class UserService : IUserService
{
    private readonly IRepository<Admin.Domain.Entities.User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IRepository<Admin.Domain.Entities.Role> _roleRepository;
    private readonly IConfigurationRepository<UserRole> _configurationRepository;
    private readonly IJwtFactory _jwtFactory;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly IDomainValidationService _domainValidationService;
    private readonly ILogRepository _logRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UserService(
        IRepository<OnAim.Admin.Domain.Entities.User> userRepository,
        IPasswordService passwordService,
        IRepository<OnAim.Admin.Domain.Entities.Role> roleRepository,
        IConfigurationRepository<UserRole> configurationRepository,
        IJwtFactory jwtFactory,
        IOtpService otpService,
        IEmailService emailService,
        IDomainValidationService domainValidationService,
        ILogRepository logRepository,
        ISecurityContextAccessor securityContextAccessor 
        )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _roleRepository = roleRepository;
        _configurationRepository = configurationRepository;
        _jwtFactory = jwtFactory;
        _otpService = otpService;
        _emailService = emailService;
        _domainValidationService = domainValidationService;
        _logRepository = logRepository;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<ApplicationResult> ActivateAccount(string email, string code)
    {
        var user = await _userRepository.Query(x => x.Email == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (user == null || user.VerificationCode != code)
            throw new BadRequestException("Invalid activation code.");

        if (user.VerificationCodeExpiration < DateTime.UtcNow)
            throw new BadRequestException("Activation code has expired.");

        user.IsActive = true;
        user.IsVerified = true;
        user.VerificationPurpose = Shared.Enums.VerificationPurpose.AccountActivation;
        user.VerificationCode = null;
        user.VerificationCodeExpiration = null;

        await _userRepository.CommitChanges();

        return new ApplicationResult { Success = true, Data = "Account activated successfully." };
    }

    public async Task<ApplicationResult> ChangePassword(string email, string oldPassword, string newPassword)
    {
        var user = await _userRepository.Query(x => x.Email == email).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found!");

        if (user?.IsActive == false)
            throw new NotFoundException("User Not Found!");

        string hashedOldPassword = _passwordService.EncryptPassword(oldPassword, user.Salt);

        if (user.Password != hashedOldPassword)
            throw new BadRequestException("Old password is incorrect!");

        var newSalt = _passwordService.Salt();
        string hashedNewPassword = _passwordService.EncryptPassword(newPassword, newSalt);

        user.Password = hashedNewPassword;
        user.Salt = newSalt;
        user.DateUpdated = SystemDate.Now;

        try
        {
            await _userRepository.CommitChanges();
        }
        catch (Exception ex)
        {
            throw new BadRequestException("An error occurred while updating the password.");
        }

        return new ApplicationResult
        {
            Success = true,
        };
    }

    public async Task<ApplicationResult> Create(string email, string firstName, string lastName, string phone)
    {
        var existingUser = await _userRepository.Query(x => x.Email == email && !x.IsDeleted).ToListAsync();

        var request = new CreateUserCommand
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone
        };

        foreach (var user in existingUser)
        {
            if (user != null)
            {
                if (user?.IsDeleted == true)
                {
                    await CreateUserWithTemporaryPassword(request);
                }
                else
                {
                    HandleExistingUser(user);
                }
            }
            else
            {
                await CreateUserWithTemporaryPassword(request);
            }
        }



        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> Delete(List<int> userIds) 
    {
        var users = await _userRepository.Query(x => userIds.Contains(x.Id)).ToListAsync();

        if (!users.Any())
            throw new NotFoundException("No users found for the provided IDs");

        foreach (var user in users)
        {
            user.IsActive = false;
            user.IsDeleted = true;
        }

        await _userRepository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> ForgotPassword(string email) 
    {
        var user = await _userRepository.Query(x => x.Email == email).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found");

        var resetCode = ActivationCodeHelper.ActivationCode().ToString();
        var resetCodeExpiration = DateTime.UtcNow.AddHours(1);

        user.VerificationCode = resetCode;
        user.VerificationCodeExpiration = resetCodeExpiration;
        user.VerificationPurpose = VerificationPurpose.PasswordReset;

        await _userRepository.CommitChanges();

        var resetLink = $"reset-password?token={resetCode}";

        var htmlBody = "" +
            "<!DOCTYPE html>\n<html>\n<head>\n    " +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            " <title>Reset Password</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";


        //string templatePath = Path.Combine("Templates", "Emails", "ForgotPassword.html");
        //string htmlBody = await ReadEmailTemplateAsync(templatePath);

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
               .Replace("{code}", resetCode.ToString())
               .Replace("{link}", resetLink);

        await _emailService.SendActivationEmailAsync(user.Email, "Password Reset", htmlBody);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> ResetPassword(string email, string code, string password)
    {
        var user = await _userRepository.Query(x =>
             x.Email == email &&
             x.VerificationPurpose == VerificationPurpose.PasswordReset &&
             x.VerificationCode == code &&
             x.VerificationCodeExpiration > DateTime.UtcNow &&
             !x.IsDeleted).FirstOrDefaultAsync();

        if (user == null)
            throw new BadRequestException("Invalid or expired reset token.");

        var salt = EncryptPasswordExtension.Salt();
        var hashedPassword = EncryptPasswordExtension.EncryptPassword(password, salt);

        user.Password = hashedPassword;
        user.Salt = salt;
        user.VerificationPurpose = VerificationPurpose.PasswordReset;
        user.VerificationCode = null;
        user.VerificationCodeExpiration = null;

        await _userRepository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<AuthResultDto> Login(LoginUserRequest model) 
    {
        var user = await _userRepository.Query(x => x.Email == model.Email && x.IsDeleted == false).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User is not active Or Doesn't Exist");

        if (user.IsActive == false)
            throw new NotFoundException("User is not active Or Doesn't Exist");

        if (user.IsVerified == false)
            throw new NotFoundException("User Not Verified");

        string hashed = EncryptPasswordExtension.EncryptPassword(model.Password, user.Salt);

        if (hashed == user.Password)
        {
            if (user.IsTwoFactorEnabled == true)
            {
                var otp = _otpService.GenerateOtp(user.Email);
                _otpService.StoreOtp(user.Id, otp);

                await _emailService.SendActivationEmailAsync(user.Email, "Your OTP Code", $"Your OTP code is: {otp}");

                return new AuthResultDto
                {
                    AccessToken = null,
                    RefreshToken = null,
                    StatusCode = 201,
                    Message = "OTP has been sent to your email."
                };
            }
            var roles = await GetUserRolesAsync(user.Id);
            var roleNames = roles.Select(r => r.Name).ToList();

            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);
            var refreshToken = await _jwtFactory.GenerateRefreshToken(user.Id);

            await _jwtFactory.SaveAccessToken(user.Id, token, DateTime.UtcNow.AddDays(1));

            user.LastLogin = SystemDate.Now;

            await _userRepository.CommitChanges();

            return new AuthResultDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                StatusCode = 200
            };
        }

        return new AuthResultDto
        {
            AccessToken = null,
            RefreshToken = null,
            StatusCode = 404
        };
    }

    public async Task<ApplicationResult> ProfileUpdate(int id, ProfileUpdateRequest profileUpdateRequest) 
    {
        var user = await _userRepository.Query(x => x.Id == id).FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User Not Found");

        user.FirstName = profileUpdateRequest.FirstName;
        user.LastName = profileUpdateRequest.LastName;
        user.Phone = profileUpdateRequest.Phone;
        await _userRepository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> Registration(string email, string password, string firstName, string lastName, string phone, DateTime DateOfBirth) 
    {
        var existingUser = await GetExistingUserAsync(email);

        if (!await _domainValidationService.IsDomainAllowedAsync(email))
            throw new BadRequestException("Domain not allowed");

        var request = new RegistrationCommand
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            DateOfBirth = DateOfBirth,
            UserName = email
        };

        if (existingUser != null)
        {
            if (existingUser.IsDeleted)
            {
                await CreateNewUser(request);
            }
            else
            {
                HandleExistingUser(existingUser);
            }
        }
        else
        {
            await CreateNewUser(request);
        }

        return new ApplicationResult { Success = true };
    }

    public async Task<AuthResultDto> TwoFA(string email, string otpCode) 
    {
        var userId = await _userRepository.Query(x => x.Email == email && x.IsDeleted == false).FirstOrDefaultAsync();
        var storedOtp = await _otpService.GetStoredOtp(userId.Id);
        if (storedOtp == otpCode)
        {
            var user = await _userRepository.Query(u => u.Id == userId.Id).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException("User doesn't exist");

            var roles = await GetUserRolesAsync(user.Id);
            var roleNames = roles.Select(r => r.Name).ToList();

            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);
            var refreshToken = await _jwtFactory.GenerateRefreshToken(user.Id);

            await _jwtFactory.SaveAccessToken(user.Id, token, DateTime.UtcNow.AddDays(1));

            user.LastLogin = SystemDate.Now;

            await _userRepository.CommitChanges();

            return new AuthResultDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                StatusCode = 200
            };
        }

        throw new UnauthorizedAccessException("Invalid OTP");
    }

    public async Task<ApplicationResult> Update(int id, UpdateUserRequest model) 
    {
        var existingUser = await _userRepository.Query(x => x.Id == id).FirstOrDefaultAsync();

        if (existingUser == null)
            throw new NotFoundException("User not found");

        existingUser.FirstName = model.FirstName;
        existingUser.LastName = model.LastName;
        existingUser.Phone = model.Phone;
        existingUser.IsActive = model.IsActive ?? true;

        var currentRoles = await _configurationRepository.Query(ur => ur.UserId == id).ToListAsync();

        var currentRoleIds = currentRoles.Select(ur => ur.RoleId).ToHashSet();
        var newRoleIds = model.RoleIds?.ToHashSet() ?? new HashSet<int>();

        var rolesToAdd = newRoleIds.Except(currentRoleIds).ToList();
        foreach (var roleId in rolesToAdd)
        {
            var userRole = new UserRole(id, roleId);
            await _configurationRepository.Store(userRole);
        }

        var rolesToRemove = currentRoleIds.Except(newRoleIds).ToList();
        foreach (var roleId in rolesToRemove)
        {
            var userRole = await _configurationRepository
                .Query(ur => ur.UserId == id && ur.RoleId == roleId).FirstOrDefaultAsync();
            if (userRole != null)
            {
                await _configurationRepository.Remove(userRole);
            }
        }

        await _userRepository.CommitChanges();
        await _configurationRepository.CommitChanges();

        return new ApplicationResult
        {
            Success = true,
            Data = $"User {existingUser.FirstName} {existingUser.LastName} Updated Successfully"
        };
    }

    public async Task<AuthResultDto> VerifyOtpAsync(int userId, string otp)
    {
        var storedOtp = await _otpService.GetStoredOtp(userId);
        if (storedOtp == otp)
        {
            var user = await _userRepository.Query(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException("User doesn't exist");

            var roles = await GetUserRolesAsync(user.Id);
            var roleNames = roles.Select(r => r.Name).ToList();

            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);
            var refreshToken = await _jwtFactory.GenerateRefreshToken(user.Id);

            await _jwtFactory.SaveAccessToken(user.Id, token, DateTime.UtcNow.AddDays(1));

            user.LastLogin = SystemDate.Now;

            await _userRepository.CommitChanges();

            return new AuthResultDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                StatusCode = 200
            };
        }

        throw new UnauthorizedAccessException("Invalid OTP");
    }

    private async Task<List<RoleResponseModel>> GetUserRolesAsync(int userId)
    {
        var result = await _configurationRepository.Query(ur => ur.UserId == userId)
                                      .Include(x => x.Role)
                                      .ThenInclude(x => x.RoleEndpointGroups)
                                      .ThenInclude(x => x.EndpointGroup)
                                      .ThenInclude(x => x.EndpointGroupEndpoints)
                                      .ThenInclude(x => x.Endpoint)
                                      .ToListAsync();

        var roles = result
            .Select(x => x.Role)
            .Distinct()
            .OrderBy(role => role.Id)
            .Select(role => new RoleResponseModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                EndpointGroupModels = role.RoleEndpointGroups.Select(z => new EndpointGroupModel
                {
                    Id = z.EndpointGroupId,
                    Name = z.EndpointGroup.Name,
                    Description = z.EndpointGroup.Description,
                    Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new EndpointRequestModel
                    {
                        Id = u.Endpoint.Id,
                        Name = u.Endpoint.Name,
                        Description = u.Endpoint.Description,

                    }).ToList()
                }).ToList()
            }).ToList();

        return roles;
    }

    private async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
    {
        var roles = await _configurationRepository.Query(ur => ur.UserId == userId).Select(ur => ur.Role).ToListAsync();

        foreach (var role in roles)
        {
            var rolesss = await _roleRepository.Query(x => x.Id == role.Id)
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .SingleOrDefaultAsync();

            foreach (var reg in rolesss.RoleEndpointGroups)
            {
                if (reg.EndpointGroup == null)
                {
                    continue;
                }

                foreach (var ege in reg.EndpointGroup.EndpointGroupEndpoints)
                {
                    if (ege.Endpoint == null)
                    {
                    }
                }
            }

        }
        var permissions = roles
                .Where(role => role != null)
                .SelectMany(role => role.RoleEndpointGroups)
                .Where(reg => reg.EndpointGroup != null)
                .SelectMany(reg => reg.EndpointGroup.EndpointGroupEndpoints)
                .Where(ege => ege.Endpoint != null && ege.Endpoint.IsDeleted)
                .Select(ege => ege.Endpoint.Path)
                .Distinct()
                .ToList();

        return permissions;
    }

    private async Task<OnAim.Admin.Domain.Entities.User> CreateUserWithTemporaryPassword(CreateUserCommand request)
    {
        var temporaryPassword = PasswordHelper.GenerateTemporaryPassword();
        var salt = EncryptPasswordExtension.Salt();
        string hashed = EncryptPasswordExtension.EncryptPassword(temporaryPassword, salt);
        var command = new CreateUserDto(request.FirstName, request.LastName, request.Email, hashed, salt, request.Phone, true, true, false);

        var user = CreateUser(command);

        await _userRepository.Store(user);
        await _userRepository.CommitChanges();

        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        await AssignRoleToUserAsync(user.Id, role.Id);

        string htmlBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  " +
            "  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n   " +
            " <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Email</title>\r\n    <style>\r\n   " +
            "     @media (prefers-color-scheme: dark) {\r\n         " +
            "   .background {\r\n                background-color: #EDEFF2 !important;\r\n      " +
            "      }\r\n\r\n            .container {\r\n                background-color: #ffffff !important;\r\n            }\r\n\r\n    " +
            "        .password-box {\r\n                background-color: rgba(0, 167, 111, 0.08) !important;\r\n            }\r\n        }\r\n " +
            "   </style>\r\n</head>\r\n<body style=\"background-color: #EDEFF2; margin: 0; padding: 0; color: #000000; font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;\">\r\n    <div class=\"background\" style=\"max-width: 558px; width: 100%; height: 80px; margin: 48px auto;\">\r\n        <img src=\"cid:logoImage\" alt=\"logo\" />\r\n    </div>\r\n\r\n    <div class=\"container\" style=\"max-width: 558px; width: 100%; padding: 48px; background-color: white; margin: auto;\">\r\n      " +
            "  <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Dear, {firstName}</p>\r\n        <p style=\"font-size: 16px; font-weight: 600; line-height: 21.79px; color: rgba(28, 37, 46, 1);\">Your Account Has Been Successfully Activated!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">Here is the temporary password you need to access your account!</p>\r\n        <p style=\"font-size: 14px; font-weight: 400; line-height: 20px; color: rgba(28, 37, 46, 1);\">For safety reasons, we recommend changing your password.</p>\r\n\r\n    " +
            "    <div class=\"password-box\" style=\"max-width: 366px; width: 100%; height: 124px; background-color: rgba(0, 167, 111, 0.08); margin: 48px auto 240px; padding: 24px 111px; text-align: center;\">\r\n            <p style=\"color: #637381; font-size: 12px; font-weight: 600; line-height: 16.34px; margin: 0;\">Temporary password</p>\r\n            <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{temporaryPassword}</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                           .Replace("{temporaryPassword}", temporaryPassword);

        await _emailService.SendActivationEmailAsync(user.Email, "Account Created", htmlBody);

        return user;
    }

    private OnAim.Admin.Domain.Entities.User CreateUser(CreateUserDto create)
    {
        return new OnAim.Admin.Domain.Entities.User(
            create.FirstName,
            create.LastName,
            create.Email,
            create.Email,
            create.Hashed,
            create.Salt,
            create.Phone,
            _securityContextAccessor.UserId,
            create.IsVerified,
            create.IsActive,
            null,
            null,
            null,
            create.IsSuperAdmin
        );
    }

    private void HandleExistingUser(OnAim.Admin.Domain.Entities.User existingUser)
    {
        if (existingUser.IsActive == false)
        {
            throw new BadRequestException($"User creation failed. The user with email: {existingUser.Email} is inactive.");
        }
        else
        {
            throw new BadRequestException($"User creation failed. A user already exists with email: {existingUser.Email}");
        }
    }

    private async Task AssignRoleToUserAsync(int userId, int roleId)
    {
        var userRole = new UserRole(userId, roleId);
        await _configurationRepository.Store(userRole);
        await _configurationRepository.CommitChanges();
    }

    private async Task<OnAim.Admin.Domain.Entities.User?> GetExistingUserAsync(string email)
    {
        return await _userRepository.Query(x => x.Email == email).FirstOrDefaultAsync();
    }

    private async Task AssignDefaultRoleToUserAsync(OnAim.Admin.Domain.Entities.User user)
    {
        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        if (role != null)
        {
            await AssignRoleToUserAsync(user.Id, role.Id);
        }
    }

    private async Task CreateNewUser(RegistrationCommand request)
    {
        var salt = EncryptPasswordExtension.Salt();
        string hashed = EncryptPasswordExtension.EncryptPassword(request.Password, salt);

        var activationCode = ActivationCodeHelper.ActivationCode().ToString();
        var activationCodeExpiration = DateTime.UtcNow.AddMinutes(15);

        var user = new OnAim.Admin.Domain.Entities.User(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Email,
            hashed,
            salt,
            request.Phone,
            _securityContextAccessor.UserId,
            isVerified: false,
            isActive: false,
            activationCode,
            VerificationPurpose.AccountActivation,
            activationCodeExpiration,
            isSuperAdmin: false
        );

        await _userRepository.Store(user);
        await _userRepository.CommitChanges();

        var role = await _roleRepository.Query(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
        await AssignRoleToUserAsync(user.Id, role.Id);

        await SendActivationEmailAsync(user, activationCode);
    }

    private async Task SendActivationEmailAsync(OnAim.Admin.Domain.Entities.User user, string activationCode)
    {
        var activationLink = $"activate?userId={user.Id}&code={activationCode}";

        string htmlBody = "" +
            "<!DOCTYPE html>\n<html>\n<head>\n    " +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    " +
            "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n   " +
            " <title>Verify Your Account</title>\n</head>\n<body style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;\">\n    " +
            "<h1>Account Verification</h1>\n    <p>Hi {firstName},</p>\n    <p>Thank you for registering. Please click the link below to verify your email address:</p>\n   " +
            " <p style=\"color: rgba(0, 167, 111, 1); font-size: 32px; font-weight: 800; line-height: 43.58px; margin: 16px 0 0;\">{code}</p>\n    <p><a href=\"{link}\">Verify your account</a></p>\n   " +
            " <p>If you didn't request this, please ignore this email.</p>\n    <p>Best regards,<br>Your Company</p>\n</body>\n</html>\n";

        htmlBody = htmlBody.Replace("{firstName}", user.FirstName)
                           .Replace("{code}", activationCode.ToString())
                           .Replace("{link}", activationLink);

        await _emailService.SendActivationEmailAsync(user.Email, "Activation Code", htmlBody);
    }

    public async Task<ApplicationResult> GetAll(UserFilter filter)
    {
        var query = _userRepository.Query(x =>
                    string.IsNullOrEmpty(filter.Name) ||
                    x.FirstName.ToLower().Contains(filter.Name.ToLower()));

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    query = query.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    query = query.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive.Value);

        if (filter.RegistrationDateFrom.HasValue)
            query = query.Where(x => x.DateCreated >= filter.RegistrationDateFrom.Value);

        if (filter.RegistrationDateTo.HasValue)
            query = query.Where(x => x.DateCreated <= filter.RegistrationDateTo.Value);

        if (filter.LoginDateFrom.HasValue)
            query = query.Where(x => x.LastLogin >= filter.LoginDateFrom.Value);

        if (filter.LoginDateTo.HasValue)
            query = query.Where(x => x.LastLogin <= filter.LoginDateTo.Value);

        if (filter.RoleIds?.Any() == true)
            query = query.Where(x => x.UserRoles.Any(ur => filter.RoleIds.Contains(ur.RoleId)));

        var paginatedResult = await Paginator.GetPaginatedResult(
            query,
            filter,
            user => new UsersModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted,
                Phone = user.Phone,
                DateCreated = user.DateCreated,
                DateUpdated = user.DateUpdated,
                Roles = user.UserRoles.Select(xx => new RoleDto
                {
                    Id = xx.RoleId,
                    Name = xx.Role.Name,
                    IsActive = xx.Role.IsActive,
                }).ToList(),
            },
            new List<string> { "Id", "FirstName", "LastName" }
            );

        return new ApplicationResult
        {
            Success = true,
            Data = paginatedResult
        };
    }

    public async Task<ApplicationResult> GetById(int id)
    {
        var query = await _userRepository
            .Query(x => x.Id == id)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ThenInclude(x => x.RoleEndpointGroups)
            .ThenInclude(x => x.EndpointGroup)
            .ThenInclude(x => x.EndpointGroupEndpoints)
            .ThenInclude(x => x.Endpoint)
            .FirstOrDefaultAsync();

        if (query == null) { return new ApplicationResult { Success = false, Data = $"User Not Found!" }; }

        var usert = await _userRepository.Query(x => x.Id == query.CreatedBy).FirstOrDefaultAsync();

        var user = new GetUserModel
        {
            Id = query.Id,
            FirstName = query.FirstName,
            LastName = query.LastName,
            Email = query.Email,
            Phone = query.Phone,
            IsActive = query.IsActive,
            UserPreferences = query.Preferences ?? new UserPreferences(),
            Roles = query.UserRoles.Select(x => new RoleModel
            {
                Id = x.RoleId,
                Name = x.Role.Name,
                IsActive = x.Role.IsActive,
                EndpointGroupModels = x.Role.RoleEndpointGroups.Select(xx => new EndpointGroupDto
                {
                    Id = xx.EndpointGroupId,
                    Name = xx.EndpointGroup.Name,
                    Description = xx.EndpointGroup.Description,
                    IsActive = xx.EndpointGroup.IsActive,
                    Endpoints = xx.EndpointGroup.EndpointGroupEndpoints.Select(xxx => new EndpointRequestModel
                    {
                        Id = xxx.Endpoint.Id,
                        Name = xxx.Endpoint.Name,
                        Path = xxx.Endpoint.Path,
                        Description = xxx.Endpoint.Description,
                        Type = ToHttpMethodExtension.ToHttpMethod(xxx.Endpoint.Type),
                        IsActive = xxx.Endpoint.IsActive,
                        IsEnabled = xxx.Endpoint.IsDeleted,
                        DateCreated = xxx.Endpoint.DateCreated,
                    }).ToList(),
                }).ToList()
            }).ToList(),
            CreatedBy = usert == null ? null : new UserDto
            {
                Id = usert.Id,
                FirstName = usert.FirstName,
                LastName = usert.LastName,
                Email = usert.Email,
            },
        };

        return new ApplicationResult
        {
            Success = true,
            Data = user,
        };
    }

    public async Task<ApplicationResult> GetUserLogs(int id, AuditLogFilter filter)
    {
        var logs = await _logRepository.GetUserLogs(id);

        if (filter.Actions != null && filter.Actions.Any())
            logs = logs.Where(x => filter.Actions.Contains(x.Action)).ToList();

        if (filter.Categories != null && filter.Categories.Any())
            logs = logs.Where(x => filter.Categories.Contains(x.Category)).ToList();

        if (filter.DateFrom.HasValue)
            logs = logs.Where(x => x.Timestamp >= filter.DateFrom.Value).ToList();

        if (filter.DateTo.HasValue)
            logs = logs.Where(x => x.Timestamp <= filter.DateTo.Value).ToList();

        var totalCount = logs.Count;

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var sortBy = filter.SortBy ?? "id";
        var sortDescending = filter.SortDescending ?? false;

        logs = SortLogs(logs, sortBy, sortDescending).ToList();

        var pagedLogs = logs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(log => new AuditLogDto
            {
                Id = log.Id,
                Action = log.Action,
                Log = log.Log,
                Timestamp = log.Timestamp
            })
            .ToList();

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<AuditLogDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = pagedLogs
            }
        };
    }

    private IEnumerable<AuditLog> SortLogs(IEnumerable<AuditLog> logs, string sortBy, bool descending)
    {
        var propertyInfo = typeof(AuditLog).GetProperty(sortBy);
        if (propertyInfo == null)
            throw new ArgumentException($"Property '{sortBy}' does not exist on type '{typeof(AuditLog).Name}'");

        var parameter = Expression.Parameter(typeof(AuditLog), "log");

        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var orderByMethod = descending
            ? nameof(Queryable.OrderByDescending)
            : nameof(Queryable.OrderBy);

        var resultExpression = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new Type[] { typeof(AuditLog), propertyInfo.PropertyType },
            logs.AsQueryable().Expression,
            Expression.Quote(orderByExpression));

        return logs.AsQueryable().Provider.CreateQuery<AuditLog>(resultExpression);
    }

    public List<dynamic> Sort(List<dynamic> input, string property)
    {
        return input.OrderBy(p => p.GetType()
                                   .GetProperty(property)
                                   .GetValue(p, null)).ToList();
    }
}
