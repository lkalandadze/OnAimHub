#nullable disable

using Shared.Domain.Entities;

namespace GameLib.Domain.Entities;

public class PrizeType : BaseEntity<int>
{
    public PrizeType()
    {
        
    }

    public PrizeType(string name, bool isMultiplied, string currencyId)
    {
        Name = name;
        IsMultiplied = isMultiplied;
        CurrencyId = currencyId;
    }

    public string Name { get; private set; }
    public bool IsMultiplied { get; private set; }

    public string CurrencyId { get; private set; }
    public Currency Currency { get; private set; }

    public void ChangeDetails(string name, bool isMultiplied, string currencyId)
    {
        Name = name;
        IsMultiplied = isMultiplied;
        CurrencyId = currencyId;
    }
}