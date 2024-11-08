using Hub.Domain.Entities;

namespace Hub.Application.Models.Tansaction;

public class TransactionResponseModel
{
    public int Id { get; set; }
    public bool Success { get; set; }
}