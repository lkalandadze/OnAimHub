using Checkmate;
using CheckmateValidations;

namespace GameLib.Domain;

//TODO: Should be deleted, Test for Gela

[CheckMate<AChecker>]
public class A
{
    public int Number { get; set; }
    public IEnumerable<B> B { get; set; }
}

public class AChecker : Checkmate<A>
{
    public AChecker()
    {
        Check(x => x.Number)
            .GreaterThan(0)
            .WithMessage("The number must be positive.");
    }
}

[CheckMate<BChecker>]
public class B
{
    public int Number { get; set; }
    public IEnumerable<C> C { get; set; }
}

public class BChecker : Checkmate<B>
{
    public BChecker()
    {
        Check(x => x.Number)
            .GreaterThan(0)
            .WithMessage("The number must be positive.");
    }
}

[CheckMate<CChecker>]
public class C
{
    public int Number { get; set; }
    public IEnumerable<D> D { get; set; }
}

public class CChecker : Checkmate<C>
{
    public CChecker()
    {
        Check(x => x.Number)
            .GreaterThan(0)
            .WithMessage("The number must be positive.");
    }
}

[CheckMate<DChecker>]
public class D
{
    public int Number { get; set; }
    public IEnumerable<E> E { get; set; }
}

public class DChecker : Checkmate<D>
{
    public DChecker()
    {
        Check(x => x.Number)
            .GreaterThan(0)
            .WithMessage("The number must be positive.");
    }
}


[CheckMate<EChecker>]
public class E
{
    public int Number { get; set; }
}

public class EChecker : Checkmate<E>
{
    public EChecker()
    {
        Check(x => x.Number)
            .GreaterThan(0)
            .WithMessage("The number must be positive.");
    }
}