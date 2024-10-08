using GameLib.Application.Models.PrizeType;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Application.Services.Concrete;

public class PrizeTypeService : IPrizeTypeService
{
    private readonly IPrizeTypeRepository _prizeTypeRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PrizeTypeService(IPrizeTypeRepository prizeTypeRepository, ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _prizeTypeRepository = prizeTypeRepository;
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PrizeTypeGetModel>> GetAllAsync()
    {
        var prizeTypes = await _prizeTypeRepository.Query()
                                                   .Include(pt => pt.Currency)
                                                   .ToListAsync();

        return prizeTypes.Select(pt => PrizeTypeGetModel.MapFrom(pt));
    }

    public async Task<PrizeTypeGetModel> GetByIdAsync(int id)
    {
        var prizeType = await _prizeTypeRepository.Query(pt => pt.Id == id)
                                                  .Include(pt => pt.Currency)
                                                  .FirstOrDefaultAsync();

        if (prizeType == null)
        {
            throw new KeyNotFoundException($"PrizeType not found for Id: {id}");
        }

        return PrizeTypeGetModel.MapFrom(prizeType);
    }

    public async Task CreateAsync(PrizeTypeCreateModel model)
    {
        var prizeType = new PrizeType(model.Name, model.IsMultiplied, model.CurrencyId);

        await _prizeTypeRepository.InsertAsync(prizeType);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(int id, PrizeTypeUpdateModel model)
    {
        var prizeType = await _prizeTypeRepository.OfIdAsync(id);

        if (prizeType == null)
        {
            throw new KeyNotFoundException($"PrizeType not found for Id: {id}");
        }

        prizeType.ChangeDetails(model.Name, model.IsMultiplied, model.CurrencyId);

        _prizeTypeRepository.Update(prizeType);
        await _unitOfWork.SaveAsync();
    }
}