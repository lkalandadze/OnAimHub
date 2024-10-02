using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Shared.Clients;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers
{
    public class BlockSegmentForPlayersCommandHandler : BaseCommandHandler<BlockSegmentForPlayersCommand, ApplicationResult>
    {
        private readonly IHubApiClient _hubApiClient;
        private readonly HubApiClientOptions _options;

        public BlockSegmentForPlayersCommandHandler(CommandContext<BlockSegmentForPlayersCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
        {
            _hubApiClient = hubApiClient;
            _options = options.Value;
        }

        protected async override Task<ApplicationResult> ExecuteAsync(BlockSegmentForPlayersCommand request, CancellationToken cancellationToken)
        {
            await ValidateAsync(request, cancellationToken);

            var req = new
            {
                SegmentId = request.SegmentId,
                File = request.File,
                ByUserId = _context.SecurityContextAccessor.UserId
            };

            var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}/Segment/{request.SegmentId}/AssignPlayers", req);

            if (result.IsSuccessStatusCode)
            {
                return new ApplicationResult { Success = true };
            }

            throw new BadRequestException("Failed to block segment for players");
        }
    }
}
