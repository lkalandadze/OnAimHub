using Microsoft.Extensions.Hosting;
using Shared.Application.Holders;

namespace Shared.Application.Services.Concrete;

public class PrizeConfiguratorService : IHostedService
{
    public GeneratorHolder Holder { get; }

    public PrizeConfiguratorService(GeneratorHolder holder)
    {
        Holder = holder;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Holder.SetGenerators();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}