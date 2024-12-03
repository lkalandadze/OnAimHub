using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

}
