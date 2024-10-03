using GameLib.Application.Models.Segment;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Application.Services.Concrete;

public class SegmentService : ISegmentService
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SegmentService(ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SegmentBaseGetModel>> GetAllAsync()
    {
        var segments = await _segmentRepository.Query()
                                               .Where(s => !s.IsDeleted)
                                               .ToListAsync();

        return segments.Select(s => SegmentBaseGetModel.MapFrom(s));
    }

    public async Task<SegmentBaseGetModel> GetByIdAsync(int id)
    {
        var segment = await _segmentRepository.OfIdAsync(id);

        if (segment == null || segment.IsDeleted)
        {
            throw new KeyNotFoundException($"Segment not found for Id: {id}");
        }

        return SegmentBaseGetModel.MapFrom(segment);
    }

    public async Task CreateAsync(SegmentCreateModel model)
    {
        var segment = new Segment(model.Id, model.ConfigurationId);

        await _segmentRepository.InsertAsync(segment);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var segment = await _segmentRepository.OfIdAsync(id);

        if (segment == null)
        {
            throw new KeyNotFoundException($"Segment not found for Id: {id}");
        }

        segment.Delete();

        _segmentRepository.Update(segment);
        await _unitOfWork.SaveAsync();
    }
}