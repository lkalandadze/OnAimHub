using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Managers;
using Shared.Application.Options;
using Shared.Domain.Abstractions;

namespace Shared.Application.Holders;

public class GeneratorHolder
{
    internal static Dictionary<BasePrizeGroup, Generator> Generators = [];
    internal List<Type> prizeGroupTypes;

    private readonly PrizeGenerationSettings _settings;
    private static object _sync = new();

    public GeneratorHolder(IOptions<PrizeGenerationSettings> settings, List<Type> prizeGroupTypes)
    {
        _settings = settings.Value;
        this.prizeGroupTypes = prizeGroupTypes;
    }

    internal void Initialize()
    {
        prizeGroupTypes.ForEach(type =>
        {
            var prizeGroups = RepositoryManager.GetPrizeGroupRepository(type).QueryWithPrizes();

            foreach (var prizeGroup in prizeGroups)
            {
                var prizeGroupType = prizeGroup.ToString()!.Split('.')[^1];

                var generator = Generator.Create(prizeGroup, _settings.PrizeGenerationType);

                Generators.Add(prizeGroup, generator);
            }
        });
    }

    public static TPrize GetPrize<TPrize>(int configId, int segmentId, Predicate<BasePrizeGroup>? predicate = null)
        where TPrize : BasePrize
    {
        var generator = GetGenerator<TPrize>(configId, segmentId, predicate);

        return (TPrize)generator.GetPrize();
    }

    internal static Generator GetGenerator<TPrize>(int configId, int segmentId, Predicate<BasePrizeGroup>? predicate)
        where TPrize : BasePrize
    {
        lock (_sync)
        {
            return Generators
                .Where(x => x.Key.Prizes.GetType().GenericTypeArguments[0] == typeof(TPrize)
                         && x.Key.SegmentId == segmentId
                         && x.Key.ConfigurationId == configId)
                .First(x => predicate?.Invoke(x.Key) ?? true)
                .Value!;
        }
    }
}