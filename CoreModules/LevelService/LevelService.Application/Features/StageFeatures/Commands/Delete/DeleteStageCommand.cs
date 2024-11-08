using MediatR;

namespace LevelService.Application.Features.StageFeatures.Commands.Delete;

public sealed class DeleteStageCommand : IRequest
{
    public int StageId { get; set; }
}