using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using MediatR;

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
            throw new Exception("HubSetting not found");

        setting.Value = request.Value;

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}