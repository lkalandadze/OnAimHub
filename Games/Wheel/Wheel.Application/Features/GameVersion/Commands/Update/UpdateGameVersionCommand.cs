
using MediatR;

namespace Wheel.Application.Features.GameVersion.Commands.Update;

public record UpdateGameVersionCommand(int Id, string Name, bool IsActive, List<int> SegmentIds) : IRequest<Unit>;