using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.UpdatePlayerBan;

public class UpdatePlayerBanCommandHandler : BaseCommandHandler<UpdatePlayerBanCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public UpdatePlayerBanCommandHandler(
        CommandContext<UpdatePlayerBanCommand> context, 
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(UpdatePlayerBanCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/UpdateBannedPlayer", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new BadRequestException("");
    }
}
