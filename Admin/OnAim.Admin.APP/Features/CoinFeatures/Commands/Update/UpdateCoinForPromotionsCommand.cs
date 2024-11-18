using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Domain.Interfaces;

namespace OnAim.Admin.APP.Features.CoinFeatures.Commands.Update;

public record UpdateCoinForPromotionsCommand(
    List<string> PromotionIds, 
    string CoinId,
    CoinDto UpdatedCoin) 
    : ICommand<ApplicationResult>;
