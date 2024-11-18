namespace OnAim.Admin.Contracts.Dtos.Coin;

public record UpdateCoinInProm(List<string> promotionIds, string coinId, CoinDto updatedCoin);
