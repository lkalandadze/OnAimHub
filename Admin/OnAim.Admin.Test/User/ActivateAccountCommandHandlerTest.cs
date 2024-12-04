using FluentValidation;
using MockQueryable;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.Activate;
using OnAim.Admin.APP.Services.Admin.AuthServices;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.Admin.User;
using OnAim.Admin.APP.Services.AdminServices.Domain;
using OnAim.Admin.APP.Services.AdminServices.EmailServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Configuration;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Test.User;

public class ActivateAccountCommandHandlerTest
{
    private readonly UserService _userService;
    protected readonly Mock<IValidator<ActivateAccountCommand>> MockValidator;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;
    private readonly Mock<IRepository<Admin.Domain.Entities.User>> _mockUserRepository;

    private readonly Mock<IPasswordService> _mockPasswordService;
    private readonly Mock<IRepository<Admin.Domain.Entities.Role>> _mockRoleRepository;
    private readonly Mock<IConfigurationRepository<UserRole>> _mockConfigurationRepository;
    private readonly Mock<IJwtFactory> _mockJwtFactory;
    private readonly Mock<IOtpService> _mockOtpService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IDomainValidationService> _mockDomainValidationService;
    private readonly Mock<ILogRepository> _mockLogRepository;

    public ActivateAccountCommandHandlerTest()
    {
        _mockUserRepository = new Mock<IRepository<Admin.Domain.Entities.User>>();
        MockValidator = new Mock<IValidator<ActivateAccountCommand>>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();
        _mockPasswordService = new Mock<IPasswordService>();
        _mockRoleRepository = new Mock<IRepository<Admin.Domain.Entities.Role>>();
        _mockConfigurationRepository = new Mock<IConfigurationRepository<UserRole>>();
        _mockJwtFactory = new Mock<IJwtFactory>();
        _mockOtpService = new Mock<IOtpService>();
        _mockEmailService = new Mock<IEmailService>();
        _mockDomainValidationService = new Mock<IDomainValidationService>();
        _mockLogRepository = new Mock<ILogRepository>();

        _userService = new UserService(
            _mockUserRepository.Object,
            _mockPasswordService.Object, 
            _mockRoleRepository.Object, 
            _mockConfigurationRepository.Object, 
            _mockJwtFactory.Object,
            _mockOtpService.Object,
            _mockEmailService.Object,
            _mockDomainValidationService.Object,
            _mockLogRepository.Object, 
            MockSecurityContextAccessor.Object
            );

        MockSecurityContextAccessor
             .Setup(x => x.UserId)
             .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ActivateAccountCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task ActivateAccount_ValidCode_ActivatesAccount()
    {
        var email = "test@example.com";
        var code = "123456";
        var user = new Admin.Domain.Entities.User
        {
            Email = email,
            VerificationCode = code,
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(10),
            IsDeleted = false
        };

        _mockUserRepository
            .Setup(x => x.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new[] { user }.AsQueryable().BuildMock());

        var result = await _userService.ActivateAccount(email, code);

        Assert.True(result.Success);
        Assert.Equal("Account activated successfully.", result.Data);
        Assert.True(user.IsActive);
        Assert.True(user.IsVerified);
        Assert.Null(user.VerificationCode);
        Assert.Null(user.VerificationCodeExpiration);
    }

    [Fact]
    public async Task ActivateAccount_InvalidCode_ThrowsBadRequestException()
    {
        var email = "test@example.com";
        var code = "wrong_code";
        var user = new Admin.Domain.Entities.User
        {
            Email = email,
            VerificationCode = "123456",
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(10),
            IsDeleted = false
        };

        _mockUserRepository
            .Setup(x => x.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new[] { user }.AsQueryable().BuildMock());

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _userService.ActivateAccount(email, code));

        Assert.Equal("Invalid activation code.", exception.Message);
    }

    [Fact]
    public async Task ActivateAccount_ExpiredCode_ThrowsBadRequestException()
    {
        var email = "test@example.com";
        var code = "123456";
        var user = new Admin.Domain.Entities.User
        {
            Email = email,
            VerificationCode = code,
            VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(-10),
            IsDeleted = false
        };

        _mockUserRepository
            .Setup(x => x.Query(It.IsAny<Expression<Func<OnAim.Admin.Domain.Entities.User, bool>>>()))
            .Returns(new[] { user }.AsQueryable().BuildMock());

        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _userService.ActivateAccount(email, code));

        Assert.Equal("Activation code has expired.", exception.Message);
    }
}
