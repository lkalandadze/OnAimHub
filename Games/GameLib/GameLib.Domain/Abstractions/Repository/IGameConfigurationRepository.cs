using GameLib.Domain.Entities;

namespace GameLib.Domain.Abstractions.Repository;

public interface IGameConfigurationRepository : IBaseRepository<GameConfiguration> 
{
    void InsertConfigurationTree(GameConfiguration aggregateRoot);

    Task UpdateConfigurationTreeAsync(GameConfiguration updatedEntity);

    void DeleteConfigurationTree(GameConfiguration aggregateRoot);

    //[Obsolete("This method is obsolete and should not be used. Please use InsertConfigurationTree(GameConfiguration aggregateRoot) instead.", true)]
    //new Task InsertAsync(GameConfiguration aggregateRoot);

    //[Obsolete("This method is obsolete and should not be used. Please use UpdateConfigurationTreeAsync(GameConfiguration updatedEntity) instead.", true)]
    //new void Update(GameConfiguration aggregateRoot);
}