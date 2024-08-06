using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Application.Generators;
using Shared.Application.Holders;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;

namespace Shared.Application.Services;

public class UpdatePrizeGroupService : IHostedService, IDisposable
{
    private readonly GeneratorHolder _holder;
    private IServiceScopeFactory _serviceScopeFactory;
    private Timer? _timer;

    public UpdatePrizeGroupService(IServiceScopeFactory serviceScopeFactory, GeneratorHolder holder)
    {
        _holder = holder;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        GeneratorHolder.Generators.ToList().ForEach(x =>
        {
            var type = x.Key.GetType();
            var generator = x.Value;

            var resopsitoryType = typeof(IPrizeGroupRepository<>).MakeGenericType(type);
            var prizeGroupRepository = scope.ServiceProvider.GetRequiredService(resopsitoryType);

            var ofIdAsyncMethod = prizeGroupRepository.GetType().GetMethod(nameof(IBaseRepository<BaseEntity>.OfIdAsync));
            var updateMethod = prizeGroupRepository.GetType().GetMethod(nameof(IBaseRepository<BaseEntity>.Update));

            var task = ofIdAsyncMethod!.Invoke(prizeGroupRepository, [x.Key.Id]) as Task;
            var resultProperty = task!.GetType().GetProperty("Result");
            var prizeGroup = resultProperty!.GetValue(task) as BasePrizeGroup;

            //var task = ofIdAsyncMethod!.Invoke(prizeGroupRepository, [x.Key.Id]) as Task<BasePrizeGroup>;
            //var prizeGroup = task.Result;

            if (generator.PrizeGenerationType == PrizeGenerationType.Sequence)
            {
                prizeGroup!.NextPrizeIndex = ((SequencePrizeGenerator)generator).NextPrizeIndex;
            }

            updateMethod.Invoke(prizeGroupRepository, [prizeGroup]);
        });

        _holder.prizeGroupTypes.ForEach(async type =>
        {
            var resopsitoryType = typeof(IPrizeGroupRepository<>).MakeGenericType(type);
            var prizeGroupRepository = scope.ServiceProvider.GetRequiredService(resopsitoryType);
            var ofIdAsyncMethod = prizeGroupRepository.GetType().GetMethod(nameof(IBaseRepository<BaseEntity>.OfIdAsync));
            var updateMethod = prizeGroupRepository.GetType().GetMethod(nameof(IBaseRepository<BaseEntity>.Update));

            var generator = GeneratorHolder.Generators.First(x => x.Key.GetType().Name == type.GetType().Name).Value;

            var task = ofIdAsyncMethod!.Invoke(prizeGroupRepository, [generator.Id]) as Task<BasePrizeGroup>;
            var prizeGroup = task!.Result;

            if (generator.PrizeGenerationType == PrizeGenerationType.Sequence)
            {
                prizeGroup.NextPrizeIndex = ((SequencePrizeGenerator)generator).NextPrizeIndex;
            }

            prizeGroup.NextPrizeIndex = 0;

            //updateMethod.Invoke(prizeGroupRepository, [prizeGroup]);
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}