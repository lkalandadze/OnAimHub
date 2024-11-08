using GameLib.Application.Managers;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Entities;

namespace GameLib.Application.Holders;

public class ConfigurationHolder
{
    internal List<Type> prizeGroupTypes;

    public IEnumerable<Price> Prices = [];
    public Dictionary<string, List<BasePrizeGroup>> PrizeGroups = [];

    public ConfigurationHolder(List<Type> prizeGroupTypes)
    {
        this.prizeGroupTypes = prizeGroupTypes;

        SetPricesAsync().Wait();
        SetPrizeGroups();
    }

    public void SetPrizeGroups()
    {
        prizeGroupTypes.ForEach(type =>
        {
            var prizeGroups = RepositoryManager.GetPrizeGroupRepository(type).QueryWithPrizes();
            PrizeGroups.Add(type.Name, prizeGroups.ToList());
        });
    }

    public async Task SetPricesAsync()
    {
        var prices = await RepositoryManager.GetPriceRepository().QueryAsync();

        if (prices != null && prices.Any())
        {
            Prices = prices.AsEnumerable();
        }
    }
}