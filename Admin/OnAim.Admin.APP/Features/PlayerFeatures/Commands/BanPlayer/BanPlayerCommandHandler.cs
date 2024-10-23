using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

public class BanPlayerCommandHandler : ICommandHandler<BanPlayerCommand, ApplicationResult>
{
    private readonly IPlayerService _playerService;
    private readonly IValidator<BanPlayerCommand> _validator;

    public BanPlayerCommandHandler(IPlayerService playerService, IValidator<BanPlayerCommand> validator)
    {
        _playerService = playerService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(BanPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _playerService.BanPlayer(request.PlayerId, request.ExpireDate, request.IsPermanent, request.Description);

        return new ApplicationResult { Success = result.Success };
    }
}
