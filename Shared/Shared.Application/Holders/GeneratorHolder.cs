using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Options;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;

namespace Shared.Application.Holders;

public class GeneratorHolder
{
    private Dictionary<Base.PrizeGroup, Generator> Generators = [];
    private object _sync = new();
    private readonly IPrizeGroupRepository _prizeGroupRepository;
    private readonly PrizeGenerationSettings _settings;
    
    public GeneratorHolder(IPrizeGroupRepository prizeGroupRepository, IOptions<PrizeGenerationSettings> settings)
    {
        _prizeGroupRepository = prizeGroupRepository;
        _settings = settings.Value;

        Init().Wait();
    }

    internal async Task Init()
    {
        var prizeGroups = await _prizeGroupRepository.QueryAsync();

        Generators = prizeGroups.ToDictionary(x => x, x =>
        {
            return Generator.Create(_prizeGroupRepository, x, _settings.PrizeGenerationType);
        });
    }

    public Base.Prize GetPrize(int versionId, int segmentId, Predicate<Base.PrizeGroup> predicate)
    {
        return GetGenerator(versionId, segmentId, predicate).GetPrize();
    }

    internal Generator GetGenerator(int versionId, int segmentId, Predicate<Base.PrizeGroup> predicate)
    {
        lock (_sync)
        {
            return Generators.First(x => x.Key.Configuration.GameVersionId == versionId &&
                                         x.Key.SegmentId == segmentId &&
                                         predicate(x.Key)).Value;
        }
    }
}