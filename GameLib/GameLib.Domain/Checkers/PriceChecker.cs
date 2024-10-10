using Checkmate;
using CheckmateValidations;
using GameLib.Domain.Entities;

namespace GameLib.Domain.Checkers;

public interface IToMessage
{
    IToSetCondition WithMessage();
}

public interface IToSetCondition
{
    IBoth SetCondition();
}

public interface IBoth : IToMessage, IToSetCondition
{

}

public class Container : IBoth
{
    public IBoth SetCondition()
    {
        throw new NotImplementedException();
    }

    public IToSetCondition WithMessage()
    {
        throw new NotImplementedException();
    }
}


public class PriceChecker : Checkmate<Price>
{
    public PriceChecker()
    {
        //var container = (new Container())
        //    .SetCondition()
        //    .SetCondition()
        //    .SetCondition()
        //    .SetCondition()
        //    .SetCondition()
        //    .WithMessage()
        //    .SetCondition()
        //    .SetCondition()
        //    .SetCondition()
        //    .SetCondition()
        //    .WithMessage();


        Check(x => x.Value)
            .SetCondition(x => x > 1)
            .SetCondition(x => x > 2)
            .WithMessage("Message 1")
            //.WithMessage("Message 1") ///!!!!!!!!!!!!!
            .SetCondition(x => x > 3)
            .SetCondition(x => x > 4)
            .SetCondition(x => x > 5)
            .WithMessage("Message 2")
            .SetCondition(x => x > 6)
            .WithMessage("Message 3");

        Check(x => x.Value)
            .SetCondition(x => x > 7)
            .WithMessage("Message 1")
            .SetCondition(x => x > 8)
            .WithMessage("Message 2");

        //Check(x => x.Multiplier)
        //    .GreaterThan(0)
        //    .GreaterThan(0)
        //    .WithMessage("The value must be positive.");

        //Check(x => x.CurrencyId)
        //    .IsNotNullOrEmpty()
        //    .WithMessage("Currency Id must not be null or empty.");
    }
}