using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Domain.Entities;
using Shared.Lib.Wrappers;

namespace GameLib.Application.Services.Abstract;

public interface IGameConfigurationService
{
    Response<EntityMetadata?> GetConfigurationMetaData();

    Task<Response<IEnumerable<GameConfiguration>>> GetAllAsync(int? configurationId, int? promotionId);

    Task<Response<GameConfiguration>> GetByIdAsync(int id);

    Task CreateAsync(GameConfiguration configuration);

    Task UpdateAsync(GameConfiguration configuration);

    Task ActivateAsync(int id);

    Task DeactivateAsync(int id);

    Task DeleteAsync(int id);

    Task DeleteByCorrelationIdAsync(Guid correlationId);
}