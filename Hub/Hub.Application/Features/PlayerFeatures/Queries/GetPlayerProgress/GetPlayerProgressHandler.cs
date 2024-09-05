using Hub.Application.Configurations;
using Hub.Application.Models.Progress;
using Hub.Application.Services.Abstract;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerProgress;

public class GetPlayerProgressHandler : IRequestHandler<GetPlayerProgressQuery, GetPlayerProgressResponse>
{
    private readonly IAuthService _authService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetPlayerProgressHandler(IAuthService authService, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _authService = authService;
        _httpClient = httpClient;
        _casinoApiConfiguration = casinoApiConfiguration.Value;
    }

    public async Task<GetPlayerProgressResponse> Handle(GetPlayerProgressQuery request, CancellationToken cancellationToken)
    {
        if (_authService.GetCurrentPlayer() == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.UnauthorizedAccessAttempt, "Unauthorized access attempt - player information is missing.");
        }

        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetBalance, _authService.GetCurrentPlayerId());

        var result = await _httpClient.CustomGetAsync<PlayerProgressGetModel>(_casinoApiConfiguration.Host, endpoint);

        if (result == null || result.Progress == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.ExternalServiceError, "Failed to retrieve player progress from casino.");
        }

        return new GetPlayerProgressResponse
        {
            Progress = result.Progress,
        };
    }
}