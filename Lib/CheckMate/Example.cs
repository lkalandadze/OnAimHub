using CheckmateValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkmate
{
    public class Example()
    {
        static void Main(string[] args)
        {
            var entity = new TestEntity();
            entity.Prop1 = 1;
            entity.Prop2 = "Pass";

            var checkers = CheckmateValidations.Checkmate.GetCheckers(entity);

            var status = CheckmateValidations.Checkmate.Check(entity);
        }
    }

    public class TestEntityChecker : Checkmate<TestEntity>
    {
        public TestEntityChecker() : base()
        {

            Check(x => x.Prop1)
                .Between(1, 20)
                .WithMessage("Must be in Range!");

            Check(x => x.Prop2.Count())
                .IsEqual(5)
                .WithMessage("Must equal Pass!");

            Check(x => x.Prop3.Count)
                .Between(1, 20)
                .WithMessage("Must be in Range!");
        }
    }

    [CheckMate<TestEntityChecker>]
    public class TestEntity
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
        public List<int> Prop3 { get; set; }
    }
}
