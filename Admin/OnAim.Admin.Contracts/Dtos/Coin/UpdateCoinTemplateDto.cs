﻿namespace OnAim.Admin.Contracts.Dtos.Coin;

public class UpdateCoinTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal? Value { get; set; }
    public CoinType CoinType { get; set; }
    public IEnumerable<int>? WithdrawOptionIds { get; set; }
    public IEnumerable<int>? WithdrawOptionGroupIds { get; set; }
}
