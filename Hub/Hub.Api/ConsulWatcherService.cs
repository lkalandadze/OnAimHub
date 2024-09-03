
using Consul;
using Hub.Application.Services.Abstract;
using Shared.Application.Models.Consul;
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
        ulong consulIndex = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var queryResult = await _consulClient.Catalog.Services(new QueryOptions
            {
                WaitIndex = consulIndex,
                WaitTime = TimeSpan.FromSeconds(5)
            }, stoppingToken);

            var services = queryResult.Response;
            var currentGameIds = new HashSet<string>();

            foreach (var service in services.Keys)
            {
                var serviceData = await _consulClient.Catalog.Service(service, string.Empty, new QueryOptions
                {
                    WaitIndex = consulIndex,
                    WaitTime = TimeSpan.FromSeconds(5)
                }, stoppingToken);

                foreach (var serviceEntry in serviceData.Response)
                {
                    if (serviceEntry.ServiceTags.Contains("Game") && serviceEntry.ServiceMeta.TryGetValue("GameData", out var gameDataJson))
                    {
                        var gameStatuses = JsonSerializer.Deserialize<List<GameRegisterResponseModel>>(gameDataJson);

                        if (gameStatuses != null)
                        {
                            foreach(var gameStatus in gameStatuses)
                            {
                                gameStatus.Address = serviceEntry.ServiceAddress;
                                _activeGameService.AddOrUpdateActiveGame(gameStatus);
                                _trackedGames[gameStatus.GameVersionId.ToString()] = serviceEntry.ServiceID;
                                currentGameIds.Add(gameStatus.GameVersionId.ToString());
                            }
                        }
                    }
                }
            }

            var removedGameIds = _trackedGames.Keys.Except(currentGameIds).ToList();
            foreach (var gameId in removedGameIds)
            {
                _activeGameService.RemoveActiveGame(gameId);
                _trackedGames.TryRemove(gameId, out _);
            }

            consulIndex = queryResult.LastIndex;
        }
    }
}