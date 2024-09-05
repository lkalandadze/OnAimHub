
using Consul;
using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Shared.Application.Models.Consul;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Hub.Api;

public class ConsulWatcherService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IActiveGameService _activeGameService;
    private readonly ConcurrentDictionary<string, GameRegisterResponseModel> _gameDataCache;
    private readonly IServiceProvider _serviceProvider;

    public ConsulWatcherService(IConsulClient consulClient, IActiveGameService activeGameService, IServiceProvider serviceProvider)
    {
        _consulClient = consulClient;
        _activeGameService = activeGameService;
        _gameDataCache = new ConcurrentDictionary<string, GameRegisterResponseModel>();
        _serviceProvider = serviceProvider;
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
                            foreach (var gameStatus in gameStatuses)
                            {
                                gameStatus.Address = serviceEntry.ServiceAddress;
                                currentGameIds.Add(gameStatus.GameVersionId.ToString());

                                if (IsNewOrUpdatedGame(gameStatus))
                                {
                                    _activeGameService.AddOrUpdateActiveGame(gameStatus);
                                    _gameDataCache[gameStatus.GameVersionId.ToString()] = gameStatus;

                                    using (var scope = _serviceProvider.CreateScope())
                                    {
                                        var gameRegistrationLogRepository = scope.ServiceProvider.GetRequiredService<IGameRegistrationLogRepository>();
                                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                                        var gameRegistrationLog = new GameRegistrationLog(gameStatus.GameVersionId, "Registered", DateTime.Now, serviceEntry.ServiceID);
                                        await gameRegistrationLogRepository.InsertAsync(gameRegistrationLog);
                                        await unitOfWork.SaveAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var removedGameIds = _gameDataCache.Keys.Except(currentGameIds).ToList();
            foreach (var gameId in removedGameIds)
            {
                _activeGameService.RemoveActiveGame(gameId);
                _gameDataCache.TryRemove(gameId, out _);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var gameRegistrationLogRepository = scope.ServiceProvider.GetRequiredService<IGameRegistrationLogRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var gameRegistrationLog = new GameRegistrationLog(int.Parse(gameId), "DeRegistered", DateTime.Now, null);
                    await gameRegistrationLogRepository.InsertAsync(gameRegistrationLog);
                    await unitOfWork.SaveAsync();
                }
            }

            consulIndex = queryResult.LastIndex;
        }
    }

    private bool IsNewOrUpdatedGame(GameRegisterResponseModel gameStatus)
    {
        if (!_gameDataCache.TryGetValue(gameStatus.GameVersionId.ToString(), out var cachedGame))
        {
            return true;
        }
        return !cachedGame.Equals(gameStatus);
    }
}