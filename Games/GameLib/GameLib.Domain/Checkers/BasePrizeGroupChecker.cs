using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Abstractions;

namespace GameLib.Domain.Checkers;

public class BasePrizeGroupChecker : Checkmate<BasePrizeGroup>
{
    public BasePrizeGroupChecker() : base()
    {
        //Check(x => x.Sequence)
        //    .SetCondition(x => x.Count() > 2)
        //    .WithMessage("The length of sequence must be at least 3.");
    }
}