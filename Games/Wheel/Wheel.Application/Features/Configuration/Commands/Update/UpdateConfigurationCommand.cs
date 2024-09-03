using MediatR;

namespace Wheel.Application.Features.Configuration.Commands.Update;

public record UpdateConfigurationCommand(int Id, string Name, bool IsDefault, bool IsActive, int GameVersionId) : IRequest<Unit>;