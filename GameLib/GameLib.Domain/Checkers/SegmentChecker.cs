using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class SegmentChecker : Checkmate<Segment>
{
    public SegmentChecker()
    {
        //Check(x => x.IsDeleted)
        //    .IsNotNull()
        //    .WithMessage("IsDeleted must not be null.");
    }
}