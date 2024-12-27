using Consul;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Hub.Api;

public class ConsulWatcherService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly IGameService _activeGameService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, string> _trackedGames;

    public ConsulWatcherService(
        IConsulClient consulClient,
        IGameService activeGameService,
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
            var currentServiceNames = new HashSet<string>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var _serviceRepository = scope.ServiceProvider.GetRequiredService<IServiceRepository>();
                var _consulLogRepository = scope.ServiceProvider.GetRequiredService<IConsulLogRepository>();

                foreach (var service in services.Response.Values)
                {
                    if (service.Tags.Contains("Game") && service.Meta.TryGetValue("GameData", out var gameDataJson))
                    {
                        var gameStatus = JsonSerializer.Deserialize<GameModel>(gameDataJson);

                        if (gameStatus != null)
                        {
                            gameStatus.Address = service.Address;
                            _activeGameService.AddOrUpdateGame(gameStatus);

                            // Check if the service has changed or is new
                            if (!_trackedGames.TryGetValue(service.Service, out var previousGameDataJson) || previousGameDataJson != gameDataJson)
                            {
                                var existingService = await _serviceRepository.Query().FirstOrDefaultAsync(s => s.Name == service.Service);

                                if (existingService == null)
                                {
                                    var newService = new Service("Game", service.Service, true);
                                    await _serviceRepository.InsertAsync(newService);
                                }
                                else
                                {
                                    existingService.IsActive = true;
                                    _serviceRepository.Update(existingService);
                                }

                                var log = new ConsulLog
                                (
                                    gameStatus.Id,
                                    service.Service,
                                    service.Port,
                                    DateTime.UtcNow
                                );

                                await _consulLogRepository.InsertAsync(log);
                                _trackedGames[service.Service] = gameDataJson;
                            }
                        }
                    }

                    currentServiceNames.Add(service.Service);
                }

                // Handle services that are no longer active
                var removedServiceNames = _trackedGames.Keys.Except(currentServiceNames).ToList();
                foreach (var serviceName in removedServiceNames)
                {
                    if (_trackedGames.TryRemove(serviceName, out _))
                    {
                        var existingService = await _serviceRepository.Query().FirstOrDefaultAsync(s => s.Name == serviceName);

                        if (existingService != null && existingService.IsActive)
                        {
                            existingService.IsActive = false;
                            _serviceRepository.Update(existingService);
                        }

                        _activeGameService.RemoveGame(serviceName);
                    }
                }

                await unitOfWork.SaveAsync();
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
