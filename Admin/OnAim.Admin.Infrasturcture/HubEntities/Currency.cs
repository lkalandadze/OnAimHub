namespace OnAim.Admin.Infrasturcture.HubEntities;

public class Currency : DbEnum<string>
{
    public const string OnAimCoinId = "onaim";

    public static Currency OnAimCoin = FromId(nameof(OnAimCoin));
    public static Currency FreeSpin = FromId("");

    public static Currency FromId(string id)
    {
        // Your logic to create a Currency from an id
        return new Currency() { Id = id }; // Adjust this to your constructor or factory method
    }
}