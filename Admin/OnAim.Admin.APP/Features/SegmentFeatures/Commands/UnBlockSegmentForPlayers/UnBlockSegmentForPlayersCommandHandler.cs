using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Services.ClientService;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net.Http.Headers;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockSegmentForPlayers;

public class UnBlockSegmentForPlayersCommandHandler : BaseCommandHandler<UnBlockSegmentForPlayersCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public UnBlockSegmentForPlayersCommandHandler(CommandContext<UnBlockSegmentForPlayersCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(UnBlockSegmentForPlayersCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        //var req = new
        //{
        //    SegmentId = request.SegmentId,
        //    File = request.File,
        //    ByUserId = _context.SecurityContextAccessor.UserId
        //};

        //var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Segment/{request.SegmentId}/UnblockPlayers", req);
        using var multipartContent = new MultipartFormDataContent();

        if (request.File != null)
        {
            var fileContent = new StreamContent(request.File.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);
            multipartContent.Add(fileContent, "file", request.File.FileName);
        }

        multipartContent.Add(new StringContent(request.SegmentId), "SegmentId");
        multipartContent.Add(new StringContent(_context.SecurityContextAccessor.UserId.ToString()), "ByUserId");

        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Admin/UnblockSegmentForPlayers?segmentId={request.SegmentId}", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to Unblock Players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(),
        };
    }
}
