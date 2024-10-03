using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;

public class BlockSegmentForPlayerCommandHandler : BaseCommandHandler<BlockSegmentForPlayerCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public BlockSegmentForPlayerCommandHandler(CommandContext<BlockSegmentForPlayerCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(BlockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var req = new
        {
            PlayerId = request.PlayerId,
            SegmentId = request.SegmentId,
            ByUserId = _context.SecurityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Segment/{request.SegmentId}/BlockPlayer/{request.PlayerId}", req);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to block segment for player");
    }
}
