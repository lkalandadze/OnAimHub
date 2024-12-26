namespace OnAim.Admin.Contracts.Dtos.Coin;

public class UpdateWithdrawOptionEndpointDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public string Content { get; set; }
    public int ContentType { get; set; }
}
