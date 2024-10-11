//TODO: Temp

using CheckmateValidations;

namespace Checkmate
{
    public class Example()
    {
        static void Ragaca(string[] args)
        {
            var entity = new TestEntity();
            entity.Prop1 = 1;
            entity.Prop2 = "Pass";

            var checkers = CheckmateValidations.Checkmate.GetCheckContainers(entity);

            var status = CheckmateValidations.Checkmate.IsValid(entity);
        }
    }

    public class TestEntityChecker : Checkmate<TestEntity>
    {
        public TestEntityChecker() : base()
        {
            //KI
            Check(x => x.Prop1)
            .SetCondition(x => x == 2)
            .WithMessage("dksjalkdjaslkdj");

            //KI
            Check(x => x.Prop1)
            .SetCondition(x => x == 2)
            .SetCondition(x => x == 2)
            .WithMessage("dksjalkdjaslkdj");

            //KI
            Check(x => x.Prop1)
            .SetCondition(x => x == 2)
            .WithMessage("dksjalkdjaslkdj")
            .SetCondition(x => x == 2)
            .WithMessage("dksjalkdjaslkdj");

            //Check(x => x.Prop1)
            //    .Between(1, 20)
            //    .WithMessage("Must be in Range!");

            //Check(x => x.Prop2)
            //    .IsEqual("")
            //    .WithMessage("Must equal Pass!");

            //Check(x => x.Prop3.FirstOrDefault())
            //    .Between(1, 20)
            //    .WithMessage("Must be in Range!");
        }
    }

    [CheckMate<TestEntityChecker>]
    public class TestEntity
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
        public List<int> Prop3 { get; set; }
    }

    [CheckMate<TestEntityChecker>]
    public class TestEntity1
    {
        public int Prop1 { get; set; }
        public TestEntity PropT { get; set; }
        public string Prop2 { get; set; }
        public List<int> Prop3 { get; set; }
    }
}
