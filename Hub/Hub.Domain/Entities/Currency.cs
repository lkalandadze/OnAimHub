using Shared.Domain.Entities;
using System.Reflection.Metadata;

namespace Hub.Domain.Entities;

public class Currency : DbEnum<string>
{

    public const string OnAimCoinId = "onaim";

    public static Currency OnAimCoin = FromId(nameof(OnAimCoin));
    public static Currency FreeSpin = Currency.FromId("");

    public static Currency FromId(string id)
    {
        // Your logic to create a Currency from an id
        return new Currency() { Id = id}; // Adjust this to your constructor or factory method
    }
}