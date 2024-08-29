using Hub.Application.Models.Tansaction;
using MediatR;

namespace Hub.Application.Features.TransactionFeatures.Commands.CreateBetTransaction;

public record CreateBetTransaction(int GameVersionId, int CurrencyId, decimal Amount) : IRequest<TransactionResponseModel>;