using AggregationService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnAim.Admin.Domain.HubEntities.Coin;
using OnAim.Admin.Domain.HubEntities.Enum;

namespace OnAim.Admin.Domain.Entities.Templates;

public class CoinTemplate : Coin
{
    public CoinTemplate(){}

    public CoinTemplate(
        string name,
        string description,
        string imageUrl,
        CoinType coinType,
        decimal? value,
        IEnumerable<CoinTemplateWithdrawOption>? withdrawOptions = null,
        IEnumerable<CoinTemplateWithdrawOptionGroup>? withdrawOptionGroups = null,
        IEnumerable<AggregationConfiguration> aggregationConfigurations = null
        )
    {
        Id = ObjectId.GenerateNewId().ToString();
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;
        Value = value ?? 0;
        WithdrawOptions = withdrawOptions?.ToList() ?? [];
        WithdrawOptionGroups = withdrawOptionGroups?.ToList();
        AggregationConfiguration = aggregationConfigurations?.ToList() ?? [];
    }

    [BsonIgnoreIfNull]
    public ICollection<CoinTemplateWithdrawOption>? WithdrawOptions { get;  set; }

    [BsonIgnoreIfNull]
    public ICollection<CoinTemplateWithdrawOptionGroup>? WithdrawOptionGroups { get; set; }

    [BsonIgnoreIfNull]
    public ICollection<AggregationConfiguration> AggregationConfiguration { get; set; }

    public decimal? Value { get; set; }
    public int Usage { get; set; }

    public void Update(string name, string description, string imageUrl, CoinType coinType, IEnumerable<CoinTemplateWithdrawOption> withdrawOptions = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;

        if (withdrawOptions != null)
        {
            UpdateWithdrawOptions(withdrawOptions);
        }
    }

    public void UpdateWithdrawOptions(IEnumerable<CoinTemplateWithdrawOption> withdrawOptions)
    {
        if (withdrawOptions != null)
        {
            WithdrawOptions.Clear();

            foreach (var option in withdrawOptions)
            {
                WithdrawOptions.Add(option);
            }
        }
    }

    public void AddWithdrawOptions(IEnumerable<CoinTemplateWithdrawOption> withdrawOptions)
    {
        if (CoinType == CoinType.Out)
        {
            WithdrawOptions ??= new List<CoinTemplateWithdrawOption>();
            foreach (var withdrawOption in withdrawOptions)
            {
                if (!WithdrawOptions.Contains(withdrawOption))
                {
                    WithdrawOptions.Add(withdrawOption);
                }
            }
        }
    }

    public void AddWithdrawOptionGroups(IEnumerable<CoinTemplateWithdrawOptionGroup> withdrawOptionGroups)
    {
        WithdrawOptionGroups ??= new List<CoinTemplateWithdrawOptionGroup>();
        foreach (var withdrawOptionGroup in withdrawOptionGroups)
        {
            if (!WithdrawOptionGroups.Contains(withdrawOptionGroup))
            {
                WithdrawOptionGroups.Add(withdrawOptionGroup);
            }
        }
    }

    public void UpdateWithdrawOptionGroups(IEnumerable<CoinTemplateWithdrawOptionGroup> withdrawOptionGroups)
    {
        if (withdrawOptionGroups != null)
        {
            WithdrawOptionGroups.Clear();

            foreach (var option in withdrawOptionGroups)
            {
                WithdrawOptionGroups.Add(option);
            }
        }
    }

    public void AddAggregationConfiguration(IEnumerable<AggregationConfiguration> aggregationConfigurations)
    {
        if (CoinType == CoinType.In)
        {
            AggregationConfiguration ??= new List<AggregationConfiguration>();
            foreach (var config in aggregationConfigurations)
            {
                if (!AggregationConfiguration.Contains(config))
                {
                    AggregationConfiguration.Add(config);
                }
            }
        }
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
