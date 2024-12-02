using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.Contracts.Dtos.Promotion;

//public class CreatePromotionDto
//{
//    public string Title { get; set; }
//    public PromotionStatus Status { get; set; }
//    public DateTimeOffset StartDate { get; set; }
//    public DateTimeOffset EndDate { get; set; }
//    public string Description { get; set; }
//    public IEnumerable<string> SegmentIds { get; set; }
//    public IEnumerable<CreatePromotionCoinModel> PromotionCoin { get; set; }
//}
public class CreatePromotionCoinModel
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsTemplate { get; set; }
    public IEnumerable<CreateWithdrawOptionGroupModel> WithdrawOptionGroups { get; set; }
}
public class CreateWithdrawOptionGroupModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<CreateWithdrawOptionModel> WithdrawOptions { get; set; }
}
public class CreateWithdrawOptionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
    public bool IsTemplate { get; set; }
}
public enum EndpointContentType
{
    FromBody = 0,
    FromQuery = 1,
}
public enum PromotionStatus
{
    NotStarted = 0,
    InProgress = 1,
    Finished = 2,
}
public class PromotionServiceDto
{
    public int PromotionId { get; set; }
    public string Type { get; set; }
    public string Service { get; set; }
    public bool IsActive { get; set; }
    public PromotionDto Promotion { get; private set; }
}
public class PromotionDto
{
    public int Id { get; set; }
    public decimal? TotalCost { get; set; }
    public PromotionStatus Status { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<PromotionServiceDto> Services { get; set; }
    public ICollection<SegmentDto> Segments { get; set; }
    public ICollection<PromotionCoinDto> PromotionCoins { get; set; }
    public ICollection<TransactionDto> Transactions { get; set; }
}
public class PromotionCoinDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public CoinType CoinType { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public int PromotionId { get; set; }
    //public PromotionDto Promotion { get; set; }

    public ICollection<WithdrawOptionDto> WithdrawOptions { get; set; }
}
public class TransactionDto
{

}
public class WithdrawOptionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }
    public int? FromTemplateId { get; private set; }

    public ICollection<WithdrawOptionGroupDto> WithdrawOptionGroups { get; set; }
}
public class WithdrawOptionGroupDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}