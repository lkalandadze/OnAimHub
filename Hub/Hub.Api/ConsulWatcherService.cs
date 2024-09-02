using Consul;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Hub.Api;

public class ConsulWatcherService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IActiveGameService _activeGameService;
    private readonly ConcurrentDictionary<string, string> _trackedGames;

    public ConsulWatcherService(IConsulClient consulClient, IActiveGameService activeGameService)
    {
        _consulClient = consulClient;
        _activeGameService = activeGameService;
        _trackedGames = new ConcurrentDictionary<string, string>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var services = await _consulClient.Agent.Services(stoppingToken);
            var currentGameIds = new HashSet<string>();

            // Update active games based on current services
            foreach (var service in services.Response.Values)
            {
                if (service.Tags.Contains("Game") && service.Meta.TryGetValue("GameData", out var gameDataJson))
                {
                    var gameStatus = JsonSerializer.Deserialize<ActiveGameModel>(gameDataJson);

                    if (gameStatus != null)
                    {
                        gameStatus.Address = service.Address;
                        _activeGameService.AddOrUpdateActiveGame(gameStatus);
                        _trackedGames[gameStatus.Id.ToString()] = service.ID;
                        currentGameIds.Add(gameStatus.Id.ToString());
                    }
                }
            }

            var removedGameIds = _trackedGames.Keys.Except(currentGameIds).ToList();
            foreach (var gameId in removedGameIds)
            {
                _activeGameService.RemoveActiveGame(gameId);
                _trackedGames.TryRemove(gameId, out _);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}