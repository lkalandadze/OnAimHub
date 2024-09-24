using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayers;

public class UnassignSegmentToPlayersHandler : IRequestHandler<UnassignSegmentToPlayersCommand>
{
    private readonly IPlayerSegmentService _playerSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnassignSegmentToPlayersHandler(IPlayerSegmentService playerSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerSegmentService = playerSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnassignSegmentToPlayersCommand request, CancellationToken cancellationToken)
    {
        

        return Unit.Value;
    }
}