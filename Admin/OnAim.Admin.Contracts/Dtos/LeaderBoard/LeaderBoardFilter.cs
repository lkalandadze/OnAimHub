using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public record LeaderBoardFilter : BaseFilter
{
    public int? LeaderBoardId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? ItemsPageNumber { get; set; }
    public int? ItemsPageSize { get; set; }
}
