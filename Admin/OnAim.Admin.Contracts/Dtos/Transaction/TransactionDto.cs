namespace OnAim.Admin.Contracts.Dtos.Transaction;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}
public class PlayerTransactionDto
{
    public int Id { get; set; }
    public string Game {  get; set; }
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public string Coin { get; set; }
    public DateTimeOffset Date { get; set; }
}