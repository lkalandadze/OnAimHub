﻿using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;

public class AssignPlayerCommandHandler : BaseCommandHandler<AssignPlayerCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public AssignPlayerCommandHandler(CommandContext<AssignPlayerCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(AssignPlayerCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var req = new 
        {
            PlayerId = request.PlayerId,
            SegmentId = request.SegmentId,
            ByUserId = _context.SecurityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJsonAndSerializeResultTo<object>(
            $"{_options.Endpoint}Admin/AssignSegmentToPlayer?segmentId={req.SegmentId}&playerId={req.PlayerId}", 
            req
            );

        if (result != null)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to assign segment");
    }
}
