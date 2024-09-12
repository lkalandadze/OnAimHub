using Consul;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Hub.Api;

public class ConsulWatcherService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IActiveGameService _activeGameService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, string> _trackedGames;

    public ConsulWatcherService(
        IConsulClient consulClient,
        IActiveGameService activeGameService,
        IServiceProvider serviceProvider)
    {
        _consulClient = consulClient;
        _activeGameService = activeGameService;
        _serviceProvider = serviceProvider;
        _trackedGames = new ConcurrentDictionary<string, string>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var services = await _consulClient.Agent.Services(stoppingToken);
            var currentGameIds = new HashSet<string>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var _consulLogRepository = scope.ServiceProvider.GetRequiredService<IConsulLogRepository>();

                foreach (var service in services.Response.Values)
                {
                    if (service.Tags.Contains("Game") && service.Meta.TryGetValue("GameData", out var gameDataJson))
                    {
                        var gameStatus = JsonSerializer.Deserialize<ActiveGameModel>(gameDataJson);

                        if (gameStatus != null)
                        {
                            gameStatus.Address = service.Address;
                            _activeGameService.AddOrUpdateActiveGame(gameStatus);

                            if (!_trackedGames.TryGetValue(service.ID, out var previousGameDataJson) || previousGameDataJson != gameDataJson)
                            {
                                var log = new ConsulLog
                                (
                                    gameStatus.Id,
                                    service.Service,
                                    service.Port,
                                    DateTime.UtcNow
                                );

                                await _consulLogRepository.InsertAsync(log);
                                await unitOfWork.SaveAsync();

                                _trackedGames[service.ID] = gameDataJson;
                            }
                        }
                    }

                    currentGameIds.Add(service.ID);
                }

                var removedGameIds = _trackedGames.Keys.Except(currentGameIds).ToList();
                foreach (var gameId in removedGameIds)
                {
                    _activeGameService.RemoveActiveGame(gameId);
                    _trackedGames.TryRemove(gameId, out _);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
