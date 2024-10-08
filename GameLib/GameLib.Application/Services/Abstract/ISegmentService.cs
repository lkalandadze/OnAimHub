using GameLib.Application.Models.Segment;

namespace GameLib.Application.Services.Abstract;

public interface ISegmentService
{
    Task<IEnumerable<SegmentBaseGetModel>> GetAllAsync();

    Task<SegmentBaseGetModel> GetByIdAsync(int id);

    Task CreateAsync(SegmentCreateModel model);

    Task DeleteAsync(int id);
}