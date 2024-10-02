using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Shared.Clients;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Net.Http;
using System;
using System.Net.Http.Headers;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;

public class AssignSegmentToPlayersCommandHandler : BaseCommandHandler<AssignSegmentToPlayersCommand, ApplicationResult>
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;

    public AssignSegmentToPlayersCommandHandler(CommandContext<AssignSegmentToPlayersCommand> context, IHubApiClient hubApiClient, IOptions<HubApiClientOptions> options) : base(context)
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
    }

    protected async override Task<ApplicationResult> ExecuteAsync(AssignSegmentToPlayersCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);

        using var multipartContent = new MultipartFormDataContent();

        // Add the file to the request
        if (request.File != null)
        {
            var fileContent = new StreamContent(request.File.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);
            multipartContent.Add(fileContent, "file", request.File.FileName);
        }

        // Add other fields to the request
        multipartContent.Add(new StringContent(request.SegmentId), "SegmentId");
        multipartContent.Add(new StringContent(_context.SecurityContextAccessor.UserId.ToString()), "ByUserId");

        // Send the multipart/form-data request
        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Segment/{request.SegmentId}/AssignPlayers", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to assign players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(), // Optionally deserialize if needed
        };
    }

}
