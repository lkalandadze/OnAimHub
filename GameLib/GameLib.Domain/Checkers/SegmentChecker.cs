using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public class SegmentChecker : Checkmate<Segment>
{
    public SegmentChecker()
    {
        Check(x => x.Id.Length)
            .GreaterThan(3)
            .WithMessage("The length of id should be more than 3.");
    }
}