using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Shared.Clients;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;

public class DeleteSegmentCommandHandler : BaseCommandHandler<DeleteSegmentCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public DeleteSegmentCommandHandler(CommandContext<DeleteSegmentCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var result = await _hubApiClient.Delete($"{_options.Endpoint}Segment/{request.Id}");

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to delete segment");
    }
}
