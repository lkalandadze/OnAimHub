namespace OnAim.Admin.Shared.DTOs.Base
{
    public record BaseFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
    }

}
