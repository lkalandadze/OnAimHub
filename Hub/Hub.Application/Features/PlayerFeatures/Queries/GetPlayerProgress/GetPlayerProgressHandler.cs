using Hub.Application.Configurations;
using Hub.Application.Models.Progress;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Extensions;

namespace Hub.Application.Features.PlayerFeatures.Queries.GetPlayerProgress;

public class GetPlayerProgressHandler : IRequestHandler<GetPlayerProgressQuery, GetPlayerProgressResponse>
{
    private readonly IAuthService _authService;
    private readonly IPlayerProgressService _playerProgressService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetPlayerProgressHandler(IAuthService authService, IPlayerProgressService playerProgressService, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _authService = authService;
        _playerProgressService = playerProgressService;
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

        await _playerProgressService.InsertOrUpdateProgressesAsync(result, _authService.GetCurrentPlayerId());

        return new GetPlayerProgressResponse
        {
            Progress = result.Progress,
        };
    }
}