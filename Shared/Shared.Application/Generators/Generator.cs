using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal abstract class Generator
{
    internal int Id { get; }
    internal List<Prize> Prizes { get; }

    internal Generator(int id, List<Prize> prizes)
    {
        Id = id;
        Prizes = prizes;
    }

    internal abstract Prize GetPrize();
}