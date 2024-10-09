using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.RevokePlayerBan;

public class RevokePlayerBanCommandHandler : BaseCommandHandler<RevokePlayerBanCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public RevokePlayerBanCommandHandler(
        CommandContext<RevokePlayerBanCommand> context,
        IHubApiClient hubApiClient, 
        IOptions<HubApiClientOptions> options
        ) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected override async Task<ApplicationResult> ExecuteAsync(RevokePlayerBanCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        var result = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/RevokePlayerBan", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new BadRequestException("");
    }
}
