using MediatR;

namespace Hub.Application.Features.SettingFeatures.Commands.Update;

public record UpdateSettingCommand(int Id, string Value) : IRequest<Unit>;