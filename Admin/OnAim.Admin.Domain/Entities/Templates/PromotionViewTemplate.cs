using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace OnAim.Admin.Domain.Entities.Templates;

public class PromotionViewTemplate
{
    public PromotionViewTemplate()
    {

    }

    public PromotionViewTemplate(string name, string url)
    {
        Name = name;
        Url = url;
    }

    [BsonId]
    [NotMapped]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    //type[] featuretemplatetype
    public bool IsDeleted { get; set; }
    public DateTimeOffset DateDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.Now;
    }

}
