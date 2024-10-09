using FluentValidation;
using Moq;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.Test;

public abstract class TestsBase<TCommand, TEntity>
    where TCommand : class
    where TEntity : class
{
    protected readonly Mock<IRepository<TEntity>> MockRepository;
    protected readonly Mock<IValidator<TCommand>> MockValidator;
    protected readonly Mock<ISecurityContextAccessor> MockSecurityContextAccessor;
    protected readonly CommandContext<TCommand> CommandContext;

    protected TestsBase()
    {
        MockRepository = new Mock<IRepository<TEntity>>();
        MockValidator = new Mock<IValidator<TCommand>>();
        MockSecurityContextAccessor = new Mock<ISecurityContextAccessor>();

        CommandContext = new CommandContext<TCommand>(
            MockValidator.Object,
            MockSecurityContextAccessor.Object
        );

        SetupDefaults();
    }

    private void SetupDefaults()
    {
        MockSecurityContextAccessor
            .Setup(x => x.UserId)
            .Returns(1);

        MockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<TCommand>(), CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }
}
