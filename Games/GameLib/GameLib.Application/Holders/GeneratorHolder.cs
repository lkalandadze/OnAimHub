using GameLib.Application.Configurations;
using GameLib.Application.Generators;
using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using Microsoft.Extensions.Options;

namespace GameLib.Application.Holders;

public class GeneratorHolder
{
    internal static Dictionary<BasePrizeGroup, Generator> Generators = [];
    internal Dictionary<Type, Type> prizePairTypes;

    private readonly PrizeGenerationConfiguration _prizeGenerationConfig;
    private static object _sync = new();

    public GeneratorHolder(IOptions<PrizeGenerationConfiguration> prizeGenerationConfig, Dictionary<Type, Type> prizePairTypes)
    {
        _prizeGenerationConfig = prizeGenerationConfig.Value;
        this.prizePairTypes = prizePairTypes;
        SetGenerators();
    }

    public void ResetGenerators()
    {
        lock (_sync)
        {
            Generators.Clear();
            SetGenerators();
        }
    }

    public static TPrize GetPrize<TPrize>(int prizeGroupId, int playerId, Predicate<BasePrizeGroup>? predicate = null)
        where TPrize : BasePrize
    {
        var generator = GetGenerator<TPrize>(prizeGroupId, predicate);
        return (TPrize)generator.GetPrize(playerId);
    }

    internal static Generator GetGenerator<TPrize>(int prizeGroupId, Predicate<BasePrizeGroup>? predicate)
        where TPrize : BasePrize
    {
        lock (_sync)
        {
            return Generators
                .Where(x => x.Key.Id == prizeGroupId)
                .First(x => predicate?.Invoke(x.Key) ?? true)
                .Value;
        }
    }

    private void SetGenerators()
    {
        foreach (var pair in prizePairTypes)
        {
            var prizeGroupType = pair.Value;
            var prizeGroups = RepositoryManager.PrizeGroupRepository(prizeGroupType).QueryWithPrizes();

            foreach (var prizeGroup in prizeGroups)
            {
                var genType = _prizeGenerationConfig.PrizeGenerationType;
                var generator = Generator.Create(prizeGroup, _prizeGenerationConfig.PrizeGenerationType);

                Generators.Add(prizeGroup, generator);
            }
        }
    }
}