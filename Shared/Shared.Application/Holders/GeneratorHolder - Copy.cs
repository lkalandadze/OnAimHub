using Microsoft.Extensions.Options;
using Shared.Application.Generators;
using Shared.Application.Options;
using Shared.Domain.Abstractions.Repository;
using Shared.Domain.Entities;
using static Shared.Domain.Entities.Base;

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

public class WheelPrizeGroup : Base.PrizeGroup<WheelPrize>
{

}

public class WheelPrize : Base.Prize
{
    public int Probability { get; set; }

    void test()
    {
       var c = new Configurator<WheelPrizeGroup, WheelPrize>(null, null);
    }
}

public class Configurator<TPrizeGroup, TPrize>
    where TPrizeGroup: Base.PrizeGroup<TPrize> where TPrize : Prize 
{
    public Configurator(IBaseRepository<TPrizeGroup> prizeGroupRepository, IOptions<PrizeGenerationSettings> settings)
    {
        var prizeGroups = prizeGroupRepository.Query().ToDictionary(x => x, x =>
        {
            return Generator.Create(prizeGroupRepository, x, settings.Value.PrizeGenerationType);
        });

        GeneratorHolder.SetConfigs(prizeGroups);
    }
}


class A
{

}

class B <T> : B where T : A
{
    public T TA { get; set; }
}

class B 
{
    public int TA1 { get; set; }

}

class C<TB, TA> where TB : B
{
    public C(TB B)
    {
        B.TA1;
    }
}



