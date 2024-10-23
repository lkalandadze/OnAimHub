using Checkmate;
using CheckmateValidations;

namespace GameLib.Domain;

//TODO: Should be deleted, Test for Gela

[CheckMate<AChecker>]
public class A
{
    public int NumberA { get; set; }
    public IEnumerable<B> B { get; set; }
}

public class AChecker : Checkmate<A>
{
    public AChecker()
    {
        Check(x => x.B)
            .SetCondition(x => x.Sum(x => x.NumberB) == 50)
            .WithMessage("(A) The sum of NumberBs should be 50.");

        Check(x => x.NumberA)
            .GreaterThan(0)
            .WithMessage("(A) The number must be positive.");
    }
}

[CheckMate<BChecker>]
public class B
{
    public string DescriptionB { get; set; }
    public int NumberB { get; set; }
    public IEnumerable<C> C { get; set; }
}

public class BChecker : Checkmate<B>
{
    public BChecker()
    {
        Check(x => x.DescriptionB.Length)
           .GreaterThan(10)
           .WithMessage("(B) The description length must be more than 10.");

        Check(x => x.NumberB)
            .GreaterThan(0)
            .WithMessage("(B) The number must be positive.");
    }
}

[CheckMate<CChecker>]
public class C
{
    public string NameC { get; set; }
    public int NumberC { get; set; }
    public IEnumerable<D> D { get; set; }
}

public class CChecker : Checkmate<C>
{
    public CChecker()
    {
        Check(x => x.NameC.Length)
            .GreaterThan(5)
            .LessThan(20)
            .WithMessage("(C) The name length must be between 5-20.");


        Check(x => x.NumberC)
            .GreaterThan(0)
            .WithMessage("(C) The number must be positive.");
    }
}

[CheckMate<DChecker>]
public class D
{
    public int NumberD { get; set; }
    public IEnumerable<E> E { get; set; }
}

public class DChecker : Checkmate<D>
{
    public DChecker()
    {
        Check(x => x.NumberD)
            .GreaterThan(0)
            .WithMessage("(D) The number must be positive.");
    }
}


[CheckMate<EChecker>]
public class E
{
    public int NumberE { get; set; }
}

public class EChecker : Checkmate<E>
{
    public EChecker()
    {
        Check(x => x.NumberE)
            .GreaterThan(0)
            .WithMessage("(E) The number must be positive.");
    }
}