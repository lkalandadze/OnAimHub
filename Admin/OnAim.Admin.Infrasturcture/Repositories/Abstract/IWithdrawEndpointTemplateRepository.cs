using MongoDB.Bson;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Abstract;

public interface IWithdrawEndpointTemplateRepository
{
    Task AddWithdrawEndpointTemplateAsync(WithdrawEndpointTemplate template);
    Task<List<WithdrawEndpointTemplate>> GetWithdrawEndpointTemplates();
    Task<WithdrawEndpointTemplate?> GetWithdrawEndpointTemplateByIdAsync(ObjectId id);
    Task<WithdrawEndpointTemplate?> UpdateWithdrawEndpointTemplateAsync(ObjectId id, WithdrawEndpointTemplate updatedWithdrawEndpointTemplate);
}
