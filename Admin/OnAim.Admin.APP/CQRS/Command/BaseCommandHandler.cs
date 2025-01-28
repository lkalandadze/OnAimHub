using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;

public abstract class BaseCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    protected readonly CommandContext<TCommand> _context;

    protected BaseCommandHandler(CommandContext<TCommand> context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        return await ExecuteAsync(request, cancellationToken);
    }

    protected abstract Task<TResponse> ExecuteAsync(TCommand request, CancellationToken cancellationToken);

    protected async Task ValidateAsync(TCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _context.Validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}
public class CommandContext<TCommand>
{
    public IValidator<TCommand> Validator { get; }
    public ISecurityContextAccessor SecurityContextAccessor { get; }

    public CommandContext(IValidator<TCommand> validator, ISecurityContextAccessor securityContextAccessor)
    {
        Validator = validator;
        SecurityContextAccessor = securityContextAccessor;
    }
}

