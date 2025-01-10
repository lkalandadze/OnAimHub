namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CreateWithdrawOptionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Value { get; set; }
    public string Endpoint { get; set; }
    public int EndpointContentType { get; set; }
    public string EndpointContent { get; set; }
    public int WithdrawOptionEndpointId { get; set; }
    public List<int> WithdrawOptionGroupIds { get; set; } = new List<int>();
}
