using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Shared.Clients;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create
{
    public sealed class CreateSegmentCommandHandler : BaseCommandHandler<CreateSegmentCommand, ApplicationResult>
    {
        private readonly IHubApiClient _hubApiClient;
        private readonly HubApiClientOptions _options;

        public CreateSegmentCommandHandler(CommandContext<CreateSegmentCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
        {
            _hubApiClient = hubApiClient;
            _options = options.Value;
        }

        protected override async Task<ApplicationResult> ExecuteAsync(CreateSegmentCommand request, CancellationToken cancellationToken)
        {
            await ValidateAsync(request, cancellationToken);
            var req = new 
            {
                Id = request.Id,
                Description = request.Description,
                PriorityLevel = request.PriorityLevel,
                CreatedByUserId = _context.SecurityContextAccessor.UserId,
            };

            var result = await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Segment", req);

            if (result != null)
            {
                return new ApplicationResult { Success = true };
            }

            throw new Exception("Failed to add segment");
        }
    }
}
