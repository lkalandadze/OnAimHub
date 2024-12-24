namespace OnAim.Admin.Contracts.Dtos.Coin;

public class CoinInTemplateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CoinType CoinType { get; set; }
    public string ImgUrl { get; set; }
    public bool IsDeleted { get; set; }
}
