using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.Domain.Entities.Templates;

public class WithdrawEndpointTemplate
{
    public WithdrawEndpointTemplate()
    {

    }

    public WithdrawEndpointTemplate(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        ContentType = contentType;
    }
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }
    [Key]
    public int WithdrawEndpointTemplateId { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public EndpointContentType ContentType { get; set; }
    public string EndpointContent { get; set; }

    public void Update(string name, string endpoint, string endpointContent, EndpointContentType contentType)
    {
        Name = name;
        Endpoint = endpoint;
        EndpointContent = endpointContent;
        ContentType = contentType;
    }
}
