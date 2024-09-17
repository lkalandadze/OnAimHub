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
    private readonly IPlayerProgressRepository _playerProgressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly ITransactionService _transactionService;
    private readonly HttpClient _httpClient;
    private readonly CasinoApiConfiguration _casinoApiConfiguration;

    public GetPlayerProgressHandler(IPlayerProgressRepository playerProgressRepository, IUnitOfWork unitOfWork, IAuthService authService, ITransactionService transactionService, HttpClient httpClient, IOptions<CasinoApiConfiguration> casinoApiConfiguration)
    {
        _playerProgressRepository = playerProgressRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _transactionService = transactionService;
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

        await InsertOrUpdateProgressesAsync(result);

        return new GetPlayerProgressResponse
        {
            Progress = result.Progress,
        };
    }

    private async Task InsertOrUpdateProgressesAsync(PlayerProgressGetModel model)
    {
        var currentProgresses = _playerProgressRepository.Query(x => x.PlayerId == _authService.GetCurrentPlayerId());

        foreach (var progress in model.Progress!)
        {
            var currentProgress = currentProgresses.FirstOrDefault(x => x.CurrencyId == progress.Key);

            if (currentProgress == null)
            {
                var newProgress = new PlayerProgress(progress.Value, _authService.GetCurrentPlayerId(), progress.Key);
                await _playerProgressRepository.InsertAsync(newProgress);

                await _transactionService.CreateTransactionAndApplyBalanceAsync(1, progress.Key, progress.Value, AccountType.Casino, AccountType.Player, TransactionType.Progress);
            }
            else
            {
                var newProgress = progress.Value - currentProgress.Progress;
                currentProgress.SetProgress(progress.Value);

                await _transactionService.CreateTransactionAndApplyBalanceAsync(1, progress.Key, newProgress, AccountType.Casino, AccountType.Player, TransactionType.Progress);
                _playerProgressRepository.Update(currentProgress);
            }
        }

        await _unitOfWork.SaveAsync();
    }
}