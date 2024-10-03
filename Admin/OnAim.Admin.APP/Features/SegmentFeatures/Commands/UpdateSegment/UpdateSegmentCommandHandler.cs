using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;

public class UpdateSegmentCommandHandler : BaseCommandHandler<UpdateSegmentCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public UpdateSegmentCommandHandler(CommandContext<UpdateSegmentCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(UpdateSegmentCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var result = await _hubApiClient.PutAsJson($"{_options.Endpoint}Segment", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to update segment");
    }
}
