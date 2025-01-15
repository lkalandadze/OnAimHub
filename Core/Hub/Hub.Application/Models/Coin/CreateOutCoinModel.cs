#nullable disable

using Hub.Domain.Enum;

namespace Hub.Application.Models.Coin;

public class CreateOutCoinModel : CreateCoinModel
{
    public decimal Value { get; set; }
    public string TemplateId { get; set; }

    public IEnumerable<int> WithdrawOptionIds { get; set; }
    public IEnumerable<int> WithdrawOptionGroupIds { get; set; }

    public CreateOutCoinModel()
    {
        CoinType = CoinType.Out;
    }
}