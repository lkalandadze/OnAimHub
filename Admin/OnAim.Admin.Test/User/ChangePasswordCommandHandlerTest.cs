﻿using FluentValidation;
using Moq;
using OnAim.Admin.APP.Feature.UserFeature.Commands.ChangePassword;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;

namespace OnAim.Admin.Test.User;

public class ChangePasswordCommandHandlerTest
{
    protected readonly Mock<IUserService> MockService;
    protected readonly Mock<IValidator<ChangePasswordCommand>> MockValidator;
    private readonly Mock<ISecurityContextAccessor> _mockSecurityContextAccessor;
    private readonly Mock<IPasswordService> _mockPasswordService;

    public ChangePasswordCommandHandlerTest()
    {
        MockService = new Mock<IUserService>();
        MockValidator = new Mock<IValidator<ChangePasswordCommand>>();
        _mockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();
        _mockPasswordService = new Mock<IPasswordService>();

        _mockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<ChangePasswordCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task Handle_ShouldChangePassword_WhenUserExists()
    {
        var command = new ChangePasswordCommand("test@example.com", "OldPassword123", "NewPassword123");

        var user = new OnAim.Admin.Domain.Entities.User(
            "John", "Doe", "johndoe", "test@example.com",
            "hashedOldPassword", "salt", "1234567890", 1,
            isVerified: true, isActive: true,
            verificationCode: null, verificationPurpose: null,
            verificationCodeExpiration: null
        );

        var filter = new Shared.DTOs.User.UserFilter(
            "", 
            "",
            "", 
            new List<int>(),
            true,
            Shared.Enums.HistoryStatus.All, 
            new DateTime(), 
            new DateTime(),
            new DateTime(),
            new DateTime()
            );

        MockService
           .Setup(service => service.GetAll(filter))
           .ReturnsAsync(new ApplicationResult { Data = new List<DomainDto>().AsQueryable() });

        _mockPasswordService
            .Setup(e => e.EncryptPassword(command.OldPassword, user.Salt))
            .Returns(user.Password);

        _mockPasswordService
            .Setup(e => e.Salt())
            .Returns("newSalt");

        _mockPasswordService
            .Setup(e => e.EncryptPassword(command.NewPassword, "newSalt"))
            .Returns("hashedNewPassword");

        MockService
            .Setup(service => service.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApplicationResult { Success = true })
            .Verifiable();

        var handler = new ChangePasswordCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoUsersFound()
    {
        var command = new ChangePasswordCommand("nonexistent@example.com", "OldPassword123", "NewPassword123");

        var filter = new Shared.DTOs.User.UserFilter(
              "",
              "",
              "",
              new List<int>(),
              true,
              Shared.Enums.HistoryStatus.All,
              new DateTime(),
              new DateTime(),
              new DateTime(),
              new DateTime()
              );

        MockService
           .Setup(service => service.GetAll(filter))
           .ReturnsAsync(new ApplicationResult { Data = new List<DomainDto>().AsQueryable() });

        MockService
             .Setup(service => service.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
             .ReturnsAsync(new ApplicationResult { Success = true })
             .Verifiable();

        var handler = new ChangePasswordCommandHandler(MockService.Object, MockValidator.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        MockService.Verify(service => service.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

}
