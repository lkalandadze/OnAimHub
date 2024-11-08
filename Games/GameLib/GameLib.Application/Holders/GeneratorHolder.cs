using Microsoft.Extensions.Options;
using GameLib.Application.Configurations;
using GameLib.Application.Generators;
using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;

namespace GameLib.Application.Holders;

public class GeneratorHolder
{
    internal static Dictionary<BasePrizeGroup, Generator> Generators = [];
    internal List<Type> prizeGroupTypes;

    private readonly PrizeGenerationConfiguration _prizeGenerationConfig;
    private static object _sync = new();

    public GeneratorHolder(IOptions<PrizeGenerationConfiguration> prizeGenerationConfig, List<Type> prizeGroupTypes)
    {
        _prizeGenerationConfig = prizeGenerationConfig.Value;
        this.prizeGroupTypes = prizeGroupTypes;
        SetGenerators();
    }

    private void SetGenerators()
    {
        prizeGroupTypes.ForEach(type =>
        {
            var prizeGroups = RepositoryManager.GetPrizeGroupRepository(type).QueryWithPrizes();

            foreach (var prizeGroup in prizeGroups)
            {
                var genType = _prizeGenerationConfig.PrizeGenerationType;
                var generator = Generator.Create(prizeGroup, _prizeGenerationConfig.PrizeGenerationType);

                Generators.Add(prizeGroup, generator);
            }
        });
    }

    public static TPrize GetPrize<TPrize>(string segmentId, Predicate<BasePrizeGroup>? predicate = null)
        where TPrize : BasePrize
    {
        var generator = GetGenerator<TPrize>(segmentId, predicate);

        return (TPrize)generator.GetPrize();
    }

    internal static Generator GetGenerator<TPrize>(string segmentId, Predicate<BasePrizeGroup>? predicate)
        where TPrize : BasePrize
    {
        lock (_sync)
        {
            return Generators
                .Where(x => x.Key.Prizes.Any() && x.Value.Prizes.Any())
                .Where(x => x.Key.Prizes.First().GetType() == typeof(TPrize))
                //.Where(x => x.Key.Configuration.Segments.Any(s => s.Id == segmentId))
                .First(x => predicate?.Invoke(x.Key) ?? true)
                .Value!;
        }
    }
}