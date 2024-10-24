using Checkmate;
using CheckmateValidations;

namespace GameLib.Domain;

//TODO: Should be deleted, Test for Gela

[CheckMate<AChecker>]
public class A
{
    public int NumberA { get; set; }
    public IEnumerable<B1> B1 { get; set; }
    public IEnumerable<B2> B2 { get; set; }
}

public class AChecker : Checkmate<A>
{
    public AChecker()
    {
        Check(x => x.B1)
            .SetCondition(x => x.Sum(x => x.NumberB1) == 50)
            .WithMessage("(A) The sum of NumberB1s should be 50.");

        Check(x => x.B2)
            .SetCondition(x => x.Sum(x => x.NumberB2) == 120)
            .WithMessage("(A) The sum of NumberB2s should be 120.");

        Check(x => x.NumberA)
            .GreaterThan(0)
            .WithMessage("(A) The number must be positive.");
    }
}

[CheckMate<B1Checker>]
public class B1
{
    public string DescriptionB1 { get; set; }
    public int NumberB1 { get; set; }
    public IEnumerable<C1> C1 { get; set; }
}

public class B1Checker : Checkmate<B1>
{
    public B1Checker()
    {
        Check(x => x.DescriptionB1.Length)
           .GreaterThan(10)
           .WithMessage("(B1) The description length must be more than 10.");

        Check(x => x.NumberB1)
            .GreaterThan(0)
            .WithMessage("(B1) The number must be positive.");
    }
}

[CheckMate<B2Checker>]
public class B2
{
    public IEnumerable<int> SequencesB2 { get; set; }
    public string DescriptionB2 { get; set; }
    public int NumberB2 { get; set; }
    public IEnumerable<C1> C1 { get; set; }
    public IEnumerable<C2> C2 { get; set; }
}

public class B2Checker : Checkmate<B2>
{
    public B2Checker()
    {
        Check(x => x.DescriptionB2.Length)
           .GreaterThan(10)
           .WithMessage("(B2) The description length must be more than 10.");

        Check(x => x.NumberB2)
            .GreaterThan(0)
            .WithMessage("(B2) The number must be positive.");
    }
}

[CheckMate<C1Checker>]
public class C1
{
    public string NameC1 { get; set; }
    public int NumberC1 { get; set; }
    public IEnumerable<D> D { get; set; }
}

public class C1Checker : Checkmate<C1>
{
    public C1Checker()
    {
        Check(x => x.NameC1.Length)
            .GreaterThan(5)
            .LessThan(20)
            .WithMessage("(C1) The name length must be between 5-20.");


        Check(x => x.NumberC1)
            .GreaterThan(0)
            .WithMessage("(C1) The number must be positive.");
    }
}

[CheckMate<C2Checker>]
public class C2
{
    public string NameC2 { get; set; }
    public int NumberC2 { get; set; }
    public IEnumerable<D> D { get; set; }
}

public class C2Checker : Checkmate<C2>
{
    public C2Checker()
    {
        Check(x => x.NameC2.Length)
            .GreaterThan(1)
            .LessThan(10)
            .WithMessage("(C2) The name length must be between 1-10.");


        Check(x => x.NumberC2)
            .GreaterThan(0)
            .WithMessage("(C2) The number must be positive.");
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