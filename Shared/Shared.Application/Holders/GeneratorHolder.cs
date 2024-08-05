using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Options;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
namespace Shared.Application.Holders;

public class GeneratorHolder
{
    private static Dictionary<BasePrizeGroup, Generator> Generators = [];
    private static object _sync = new();

    private readonly IBaseRepository<BasePrizeGroup> _prizeGroupRepository;
    private readonly PrizeGenerationSettings _settings;

    public GeneratorHolder(IBaseRepository<BasePrizeGroup> prizeGroupRepository, IOptions<PrizeGenerationSettings> settings)
    {
        _prizeGroupRepository = prizeGroupRepository;
        _settings = settings.Value;

        InitializeAsync().Wait();
    }

    private async Task InitializeAsync()
    {
        var prizeGroups = await _prizeGroupRepository.QueryAsync();

        Generators = prizeGroups.ToDictionary(x => x, x =>
        {
            return Generator.Create(_prizeGroupRepository, x, _settings.PrizeGenerationType);
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