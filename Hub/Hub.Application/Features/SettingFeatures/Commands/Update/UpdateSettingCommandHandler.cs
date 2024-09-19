using Hub.Domain.Absractions;
using MediatR;

namespace Hub.Application.Features.SettingFeatures.Commands.Update;

public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Unit>
{
    private readonly ISettingRepository _settingRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateSettingCommandHandler(ISettingRepository settingRepository, IUnitOfWork unitOfWork)
    {
        _settingRepository = settingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = _settingRepository.Query().FirstOrDefault(x => x.Id == request.Id);

        if (setting == default)
            throw new Exception("Setting not found");

        setting.Update(request.Value);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}