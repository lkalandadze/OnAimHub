using System.Text.Json.Serialization;

namespace Hub.Application.Models.Balance;

public class BalanceGetModel
{
    [JsonPropertyName("balances")]
    public Dictionary<string, double> Balances { get; set; }
}