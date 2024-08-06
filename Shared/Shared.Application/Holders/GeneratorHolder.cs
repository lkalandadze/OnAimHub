using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Options;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
namespace Shared.Application.Holders;

public class GeneratorHolder
{
    internal static Dictionary<BasePrizeGroup, Generator> Generators = [];
    internal List<Type> prizeGroupTypes;
    private static object _sync = new();

    private readonly PrizeGenerationSettings _settings;

    private IServiceScopeFactory _serviceScopeFactory;

    public GeneratorHolder(IServiceScopeFactory serviceScopeFactory, IOptions<PrizeGenerationSettings> settings, List<Type> prizeGroupTypes)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _settings = settings.Value;
        this.prizeGroupTypes = prizeGroupTypes;
    }

    internal void Initialize()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        prizeGroupTypes.ForEach(type =>
        {
            var genericType = typeof(IPrizeGroupRepository<>).MakeGenericType(type);
            var prizeGroupRepository = scope.ServiceProvider.GetRequiredService(genericType) as IBaseRepository;

            var queryAsyncMethod = prizeGroupRepository.GetType().GetMethod(nameof(IPrizeGroupRepository<BasePrizeGroup>.QueryWithPrizes));

            if (queryAsyncMethod != null)
            {
                var prizeGroups = (IEnumerable<BasePrizeGroup>)queryAsyncMethod.Invoke(prizeGroupRepository, [])!;

                foreach (var prizeGroup in prizeGroups)
                {
                    var prizeGroupType = prizeGroup.ToString()!.Split('.')[^1];

                    var generator = Generator.Create(prizeGroup, _settings.PrizeGenerationType);

                    Generators.Add(prizeGroup, generator);
                }
            }
        });
    }

    public static TPrize GetPrize<TPrize>(int configId, int segmentId, Predicate<BasePrizeGroup> predicate)
        where TPrize : BasePrize
    {
        var generator = GetGenerator<TPrize>(configId, segmentId, predicate);

        return (TPrize)generator.GetPrize();
    }

    internal static Generator GetGenerator<TPrize>(int configId, int segmentId, Predicate<BasePrizeGroup> predicate)
        where TPrize : BasePrize
    {
        lock (_sync)
        {
            return Generators
                .Where(x => x.Key.Prizes.GetType().GenericTypeArguments[0] == typeof(TPrize)
                         && x.Key.SegmentId == segmentId
                         && x.Key.ConfigurationId == configId)
                .First(x => (predicate?.Invoke(x.Key) ?? true))
                .Value!;
        }
    }
}