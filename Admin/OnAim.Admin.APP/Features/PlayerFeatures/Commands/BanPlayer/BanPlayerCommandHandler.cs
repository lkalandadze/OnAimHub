using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Shared.Clients;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Commands.BanPlayer
{
    public class BanPlayerCommandHandler : BaseCommandHandler<BanPlayerCommand, ApplicationResult>
    {
        private readonly IHubApiClient _hubApiClient;
        private readonly HubApiClientOptions _options;

        public BanPlayerCommandHandler(CommandContext<BanPlayerCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
        {
            _hubApiClient = hubApiClient;
            _options = options.Value;
        }
        protected override async Task<ApplicationResult> ExecuteAsync(BanPlayerCommand request, CancellationToken cancellationToken)
        {
            await ValidateAsync(request, cancellationToken);

            var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}/Player/BanPlayer", request);

            if (result.IsSuccessStatusCode)
            {
                return new ApplicationResult { Success = true };
            }

            throw new BadRequestException("Failed to Ban player");
        }
    }
}
