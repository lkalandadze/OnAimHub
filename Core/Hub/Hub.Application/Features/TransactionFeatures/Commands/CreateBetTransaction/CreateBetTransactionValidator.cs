using FluentValidation;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public class CreateBetTransactionValidator : AbstractValidator<CreateBetTransactionCommand>
{
    public CreateBetTransactionValidator()
    {
        //TODO
        RuleFor(x => x.Amount);
    }
}