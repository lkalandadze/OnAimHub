using GameLib.Application.Models.Segment;

namespace GameLib.Application.Services.Abstract;

public interface ISegmentService
{
    Task<IEnumerable<SegmentGetModel>> GetAllAsync();

    Task<SegmentGetModel> GetByIdAsync(int id);

    Task CreateAsync(SegmentCreateModel model);

    Task DeleteAsync(int id);
}