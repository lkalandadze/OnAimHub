using Microsoft.Extensions.Hosting;
using Shared.Application.Generators;
using Shared.Application.Holders;
using Shared.Application.Managers;

namespace Shared.Application.Services;

public class UpdatePrizeGroupService : IHostedService
{
    private readonly GeneratorHolder _holder;

    public UpdatePrizeGroupService(GeneratorHolder holder)
    {
        _holder = holder;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        GeneratorHolder.Generators.ToList().ForEach(async x =>
        {
            var type = x.Key.GetType();
            var generator = x.Value;

            var prizeGroup = await RepositoryManager.GetPrizeGroupRepository(type).OfIdAsync(x.Key.Id);

            if (generator.PrizeGenerationType == PrizeGenerationType.Sequence)
            {
                prizeGroup.NextPrizeIndex = ((SequencePrizeGenerator)generator).NextPrizeIndex;

                RepositoryManager.GetPrizeGroupRepository(type).Update(prizeGroup);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}