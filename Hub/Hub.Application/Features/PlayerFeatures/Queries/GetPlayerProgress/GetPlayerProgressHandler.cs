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
    private readonly IPlayerProgressService _playerProgressService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;
    private static readonly Dictionary<int, SemaphoreSlim> PlayerLocks = [];
    private static readonly object LockObject = new();

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

        var playerId = _authService.GetCurrentPlayerId();
        var endpoint = string.Format(_casinoApiConfiguration.Endpoints.GetBalance, playerId);
        var result = await _httpClient.CustomGetAsync<PlayerProgressGetModel>(_casinoApiConfiguration.Host, endpoint);

        if (result == null || result.Progress == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.ExternalServiceError, "Failed to retrieve player progress from casino.");
        }

        // Ensure the SemaphoreSlim for the player exists and acquire it
        SemaphoreSlim? playerSemaphore;
        lock (LockObject)
        {
            if (!PlayerLocks.TryGetValue(playerId, out playerSemaphore))
            {
                playerSemaphore = new SemaphoreSlim(1, 1);
                PlayerLocks[playerId] = playerSemaphore;
            }
        }

        // Wait asynchronously to acquire the semaphore
        await playerSemaphore.WaitAsync(cancellationToken);
        try
        {
            await _playerProgressService.InsertOrUpdateProgressesAsync(result, playerId);
        }
        finally
        {
            // Always release the semaphore
            playerSemaphore.Release();

            // Optional: Remove semaphore from the dictionary if it's no longer in use
            lock (LockObject)
            {
                if (playerSemaphore.CurrentCount == 1) // If no one else is waiting for this semaphore
                {
                    PlayerLocks.Remove(playerId);
                }
            }
        }

        return new GetPlayerProgressResponse
        {
            Progress = result.Progress,
        };
    }
}