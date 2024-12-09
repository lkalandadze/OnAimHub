using GameLib.Application.Models.PrizeType;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Wrappers;

namespace GameLib.Application.Services.Concrete;

public class PrizeTypeService : IPrizeTypeService
{
    private readonly IPrizeTypeRepository _prizeTypeRepository;
    private readonly ICoinRepository _coinRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PrizeTypeService(IPrizeTypeRepository prizeTypeRepository, ICoinRepository coinRepository, IUnitOfWork unitOfWork)
    {
        _prizeTypeRepository = prizeTypeRepository;
        _coinRepository = coinRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IEnumerable<PrizeTypeGetModel>>> GetAllAsync()
    {
        var prizeTypes = await _prizeTypeRepository.Query()
                                                   .Include(pt => pt.Coin)
                                                   .ToListAsync();

        return new Response<IEnumerable<PrizeTypeGetModel>> { Data = prizeTypes.Select(pt => PrizeTypeGetModel.MapFrom(pt)) };
    }

    public async Task<Response<PrizeTypeGetModel>> GetByIdAsync(int id)
    {
        var prizeType = await _prizeTypeRepository.Query(pt => pt.Id == id)
                                                  .Include(pt => pt.Coin)
                                                  .FirstOrDefaultAsync();

        if (prizeType == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"PrizeType with the specified ID: [{id}] was not found.");
        }

        return new Response<PrizeTypeGetModel> { Data = PrizeTypeGetModel.MapFrom(prizeType) };
    }

    public async Task CreateAsync(PrizeTypeCreateModel model)
    {
        if (await _coinRepository.OfIdAsync(model.CoinId) is null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Coin with ID '{model.CoinId}' was not found.");
        }

        var prizeType = new PrizeType(model.Name, model.IsMultiplied, model.CoinId);

        await _prizeTypeRepository.InsertAsync(prizeType);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(int id, PrizeTypeUpdateModel model)
    {
        if (await _coinRepository.OfIdAsync(model.CoinId) is null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Coin with ID '{model.CoinId}' was not found.");
        }

        var prizeType = await _prizeTypeRepository.OfIdAsync(id);

        if (prizeType == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"PrizeType with the specified ID: [{id}] was not found.");
        }

        prizeType.ChangeDetails(model.Name, model.IsMultiplied, model.CoinId);

        _prizeTypeRepository.Update(prizeType);
        await _unitOfWork.SaveAsync();
    }
}