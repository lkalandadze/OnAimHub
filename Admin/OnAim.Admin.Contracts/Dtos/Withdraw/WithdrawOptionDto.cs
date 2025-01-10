namespace OnAim.Admin.Contracts.Dtos.Withdraw;

public class WithdrawOptionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public decimal Value { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
    public int? WithdrawOptionEndpointId { get; set; }
    public ICollection<WithdrawOptionGroupDto> WithdrawOptionGroups { get; set; }
    public ICollection<OutCoinDto> OutCoins { get; set; }
}