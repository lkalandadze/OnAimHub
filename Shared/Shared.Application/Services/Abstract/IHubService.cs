namespace Shared.Application.Services.Abstract;

public interface IHubService
{
    Task BetTransactionAsync(int gameVersionId);

    Task WinTransactionAsync(int gameVersionId);
}