using GameLib.Application.Configurations;
using GameLib.Application.Generators;
using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using Microsoft.Extensions.Options;

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
            var prizeGroups = RepositoryManager.PrizeGroupRepository(type).QueryWithPrizes();

            foreach (var prizeGroup in prizeGroups)
            {
                var genType = _prizeGenerationConfig.PrizeGenerationType;
                var generator = Generator.Create(prizeGroup, _prizeGenerationConfig.PrizeGenerationType);

                Generators.Add(prizeGroup, generator);
            }
        });
    }

    public static async Task<TPrize> GetPrizeAsync<TPrize>(int prizeGroupId, int? playerId = null, Predicate<BasePrizeGroup>? predicate = null)
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
                .Where(x => x.Key.GetBasePrizes().Any() && x.Value.Prizes.Any())
                .Where(x => x.Key.GetBasePrizes().First().GetType() == typeof(TPrize))
                .First(x => predicate?.Invoke(x.Key) ?? true)
                .Value!;
        }
    }
}