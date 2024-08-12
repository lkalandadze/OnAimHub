namespace Hub.Application.Models.Balance;

public class BalanceGetModel
{
    public int PlayerId { get; set; }
    public Dictionary<string, double> Balances { get; set; }
}