using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using Shared.Infrastructure.Bus;
using Shared.IntegrationEvents.IntegrationEvents.Player;

public class BanPlayerCommandHandler : ICommandHandler<BanPlayerCommand, ApplicationResult>
{
    private readonly IMessageBus _bus;
    private readonly IValidator<BanPlayerCommand> _validator;

    public BanPlayerCommandHandler(IMessageBus bus, IValidator<BanPlayerCommand> validator)
    {
        _bus = bus;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(BanPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var playerEvent = new BanPlayerEvent(
            playerId: request.PlayerId,
            expireDate: request.ExpireDate,
            isPermanent: request.IsPermanent,
            description: request.Description);

        await _bus.Publish(playerEvent);

        return new ApplicationResult { Success = true };
    }
}
