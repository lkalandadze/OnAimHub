using Shared.Domain.Entities;

namespace Shared.Application.Generators;

internal abstract class Generator
{
    internal int Id { get; }
    internal List<Base.Prize> Prizes { get; }

    internal Generator(int id, List<Base.Prize> prizes)
    {
        Id = id;
        Prizes = prizes;
    }

    internal abstract Base.Prize GetPrize();
}