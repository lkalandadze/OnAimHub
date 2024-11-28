using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Domain.Entities.Templates;

public class CoinTemplate
{
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    [Key]
    public int CoinTemplateId { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get;  set; }
    public CoinType CoinType { get;  set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }

    public ICollection<WithdrawOption>? WithdrawOptions { get;  set; }

    public ICollection<WithdrawOptionGroup>? WithdrawOptionGroups { get; set; }

    public void Update(string name, string description, string imageUrl, CoinType coinType, IEnumerable<WithdrawOption> withdrawOptions = null)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CoinType = coinType;

        if (withdrawOptions != null)
        {
            SetWithdrawOptions(withdrawOptions);
        }
    }

    public void SetWithdrawOptions(IEnumerable<WithdrawOption> withdrawOptions)
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

    public void AddWithdrawOptions(IEnumerable<WithdrawOption> withdrawOptions)
    {
        WithdrawOptions ??= new List<WithdrawOption>();
        foreach (var withdrawOption in withdrawOptions)
        {
            if (!WithdrawOptions.Contains(withdrawOption))
            {
                WithdrawOptions.Add(withdrawOption);
            }
        }
    }

    public void AddWithdrawOptionGroups(IEnumerable<WithdrawOptionGroup> withdrawOptionGroups)
    {
        WithdrawOptionGroups ??= new List<WithdrawOptionGroup>();
        foreach (var withdrawOptionGroup in withdrawOptionGroups)
        {
            if (!WithdrawOptionGroups.Contains(withdrawOptionGroup))
            {
                WithdrawOptionGroups.Add(withdrawOptionGroup);
            }
        }
    }

    public void SetWithdrawOptionGroups(IEnumerable<WithdrawOptionGroup> withdrawOptionGroups)
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

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
