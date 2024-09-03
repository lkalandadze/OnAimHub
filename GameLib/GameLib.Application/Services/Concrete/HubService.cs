using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using GameLib.Application.Configurations;
using GameLib.Application.Models.Transaction;
using GameLib.Application.Services.Abstract;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;

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

    public async Task BetTransactionAsync(int gameVersionId)
    {
        Authorize();
        var transactionPost = new TransactionPostModel
        {
            GameId = gameVersionId,
            CurrencyId = "USD", //TODO
            Amount = 1, //TODO
        };

        var betTransaction = await _httpClient.CustomPostAsync<TransactionResponseModel>(_hubApiConfig.Host, _hubApiConfig.Endpoints.BetTransaction, transactionPost);

        if (betTransaction == null || !betTransaction.Success)
        {
            throw new Exception();
        }
    }

    public async Task WinTransactionAsync(int gameVersionId)
    {
        Authorize();
        var transactionPost = new TransactionPostModel
        {
            GameId = gameVersionId,
            CurrencyId = "USD", //TODO
            Amount = 50, //TODO
        };

        var winTransaction = await _httpClient.CustomPostAsync<TransactionResponseModel>(_hubApiConfig.Host, _hubApiConfig.Endpoints.WinTransaction, transactionPost);

        if (winTransaction == null || !winTransaction.Success)
        {
            throw new Exception();
        }
    }

    private void Authorize()
    {
        Authorize();
        var authHeader = _accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();

        if (string.IsNullOrEmpty(authHeader))
        {
            throw new InvalidOperationException();
        }

        var token = authHeader.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}