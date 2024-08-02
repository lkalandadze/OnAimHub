using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Options;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;

namespace Shared.Application.Holders;

public class GeneratorHolder
{
    private static Dictionary<Base.PrizeGroup, Generator> Generators = [];
    private static object _sync = new();
    //private readonly IPrizeGroupRepository _prizeGroupRepository;
    //private readonly PrizeGenerationSettings _settings;

    internal static void SetConfigs(Dictionary<Base.PrizeGroup, Generator> generators)
    {
        Generators = generators;
    }

    public static Base.Prize GetPrize(int versionId, int segmentId, Predicate<Base.PrizeGroup> predicate)
    {
        return GetGenerator(versionId, segmentId, predicate).GetPrize();
    }

    internal static Generator GetGenerator(int versionId, int segmentId, Predicate<Base.PrizeGroup> predicate)
    {
        lock (_sync)
        {
            return Generators.First(x => x.Key.Configuration.GameVersionId == versionId &&
                                         x.Key.SegmentId == segmentId &&
                                         predicate(x.Key)).Value;
        }
    }
}

public class Configurator
{
    public Configurator(IPrizeGroupRepository prizeGroupRepository, IOptions<PrizeGenerationSettings> settings)
    {
        var prizeGroups = prizeGroupRepository.Query().ToDictionary(x => x, x =>
        {
            return Generator.Create(prizeGroupRepository, x, settings.Value.PrizeGenerationType);
        });

        GeneratorHolder.SetConfigs(prizeGroups);
    }
}