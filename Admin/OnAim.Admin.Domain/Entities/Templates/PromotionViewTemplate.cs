using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnAim.Admin.Domain.HubEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Domain.Entities.Templates;

public class PromotionViewTemplate
{
    public PromotionViewTemplate()
    {

    }

    public PromotionViewTemplate(string name, string url, IEnumerable<PromotionView> promotionViews = null)
    {
        Name = name;
        Url = url;
        PromotionViews = promotionViews?.ToList() ?? [];
    }
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }
    [Key]
    public int PromotionViewTemplateId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

    public ICollection<PromotionView> PromotionViews { get; set; }
}
