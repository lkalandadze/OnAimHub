using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.SettingFeatures.Commands.Update;

public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Unit>
{
    private readonly IHubSettingRepository _settingRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateSettingCommandHandler(IHubSettingRepository settingRepository, IUnitOfWork unitOfWork)
    {
        _settingRepository = settingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = _settingRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (setting == default)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Setting with the specified ID: [{request.Id}] was not found.");
        }

        setting.Value = request.Value;

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}