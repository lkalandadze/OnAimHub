using GameLib.Application.Models.PrizeType;

namespace GameLib.Application.Services.Abstract;

public interface IPrizeTypeService
{
    Task<IEnumerable<PrizeTypeGetModel>> GetAllAsync();

    Task<PrizeTypeGetModel> GetByIdAsync(int id);

    Task CreateAsync(PrizeTypeCreateModel model);

    Task UpdateAsync(int id, PrizeTypeUpdateModel model);
}