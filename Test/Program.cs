using Checkmate;
using CheckmateValidations;

var A = new A()
{
    Prop1 = 3,
    B = new B()
    {
        Prop1 = 2,
        Prop2 = 333,
        C = new C()
        {
            Prop1 = 1,
        },
        Cs = 
        [
            new() { Prop1 = 10,
                    Ds = [
                        new() { Prop1 = 5 },
                        new() { Prop1 = 15 },
                        new() { Prop1 = 25 },
                    ]},
            new() { Prop1 = 22,
                    Ds = [
                        new() { Prop1 = 3 },
                        new() { Prop1 = 13 },
                        new() { Prop1 = 23 },
                    ]},
        ]
    },
};

var checkersA = CheckmateValidations.Checkmate.GetCheckContainers(A);

var validations = CheckmateValidations.Checkmate.GetCheckContainers(A).Select(x => x.Messages).ToList();

var failedCheckersA = CheckmateValidations.Checkmate.GetFailedChecks(A);

var statusA = CheckmateValidations.Checkmate.IsValid(A);

Console.WriteLine();

public class AChecker : Checkmate<A>
{
    public AChecker() : base()
    {
        //Check(A => A.Prop1)
        //    .IsEqual(3)
        //    .WithMessage("Class A, Prop1");

        //Check(A => A.Prop1)
        //    .LessThan(2)
        //    .WithMessage("Class A, Prop1");

        //Check(A => A.Prop1)
        //    .Between(3, 8)
        //    .WithMessage("Class A, Prop1");

        Check(A => A.B.Cs[1].Prop1)
            .Between(8, 15)
            .WithMessage("A.B.Cs[1].Prop1 sad asdasdasd");

        Check(A => A.B.Cs[1].Ds[0].Prop1)
            .IsEqual(3)
            .WithMessage("A.B.Cs[1].Ds[0].Prop1");
    }
}

[CheckMate<AChecker>]
public class A
{
    public int Prop1 { get; set; }
    public B B { get; set; }
}

public class B
{
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public C C { get; set; }
    public List<C> Cs { get; set; }
}

public class C
{
    public int Prop1 { get; set; }
    public List<D> Ds { get; set; }
}

public class D
{
    public int Prop1 { get; set; }
}