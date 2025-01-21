using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using GameLib.Application.Configurations;
using GameLib.Application.Models.Transaction;
using GameLib.Application.Services.Abstract;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;
using Shared.Application.Exceptions.Types;
using Shared.Application.Exceptions;

namespace GameLib.Application.Services.Concrete;

public class HubService : IHubService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly HttpClient _httpClient;
    private readonly HubApiConfiguration _hubApiConfig;

    public HubService(IHttpContextAccessor accessor, HttpClient httpClient, IOptions<HubApiConfiguration> hubApiConfig)
    {
        _accessor = accessor;
        _httpClient = httpClient;
        _hubApiConfig = hubApiConfig.Value;
    }

    public async Task BetTransactionAsync(int gameVersionId, string sourceServiceName, int promotionId, decimal amount)
    {
        Authorize();
        var transactionPost = new TransactionPostModel
        {
            KeyId = gameVersionId,
            SourceServiceName = sourceServiceName,
            PromotionId = promotionId,
            Amount = amount,
        };

        try
        {
            var betTransaction = await _httpClient.CustomPostAsync<TransactionResponseModel>(_hubApiConfig.Host, _hubApiConfig.Endpoints.BetTransaction, transactionPost);

            if (betTransaction == null)
            {
                throw new ApiException(ApiExceptionCodeTypes.DependencyFailure, "Failed to process bet transaction due to null response from the service.");
            }

            if (!betTransaction.Success)
            {
                var reason = /*betTransaction.ErrorCode ?? */"Unknown error";
                throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"BetAmount transaction failed: {reason}.");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

        
    }

    public async Task WinTransactionAsync(int gameVersionId, string sourceServiceName, string coinId, int promotionId, decimal amount)
    {
        Authorize();
        var transactionPost = new TransactionPostModel
        {
            KeyId = gameVersionId,
            SourceServiceName = sourceServiceName,
            CoinId = coinId,
            PromotionId = promotionId,
            Amount = amount,
        };

        var winTransaction = await _httpClient.CustomPostAsync<TransactionResponseModel>(_hubApiConfig.Host, _hubApiConfig.Endpoints.WinTransaction, transactionPost);

        if (winTransaction == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.DependencyFailure, "Failed to process win transaction due to null response from the service.");
        }

        if (!winTransaction.Success)
        {
            var reason = /*winTransaction.ErrorCode ?? */"Unknown error";
            throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"Win transaction failed: {reason}.");
        }
    }

    private void Authorize()
    {
        var authHeader = _accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

        if (string.IsNullOrEmpty(authHeader))
        {
            throw new ApiException(ApiExceptionCodeTypes.AuthenticationFailed, "Authorization header is missing.");
        }

        var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(token))
        {
            throw new ApiException(ApiExceptionCodeTypes.AuthenticationFailed, "Authorization token is missing or invalid.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}