using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;

public class UpdatePlayerBanCommandHandler : ICommandHandler<UpdatePlayerBanCommand, ApplicationResult>
{
    private readonly IPlayerService _playerService;
    private readonly IValidator<UpdatePlayerBanCommand> _validator;

    public UpdatePlayerBanCommandHandler(IPlayerService playerService, IValidator<UpdatePlayerBanCommand> validator) 
    {
        _playerService = playerService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UpdatePlayerBanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _playerService.UpdateBan(request.Id, request.ExpireDate, request.IsPermanent, request.Description);

        return new ApplicationResult { Success = result.Success };
    }
}
