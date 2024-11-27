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

    public ICollection<WithdrawOptionAdmin>? WithdrawOptions { get;  set; }

    public void Update(string name, string description, string imageUrl, CoinType coinType, IEnumerable<WithdrawOptionAdmin> withdrawOptions = null)
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

    public void SetWithdrawOptions(IEnumerable<WithdrawOptionAdmin> withdrawOptions)
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

    public void AddWithdrawOptions(IEnumerable<WithdrawOptionAdmin> withdrawOptions)
    {
        WithdrawOptions ??= new List<WithdrawOptionAdmin>();
        foreach (var withdrawOption in withdrawOptions)
        {
            if (!WithdrawOptions.Contains(withdrawOption))
            {
                WithdrawOptions.Add(withdrawOption);
            }
        }
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
