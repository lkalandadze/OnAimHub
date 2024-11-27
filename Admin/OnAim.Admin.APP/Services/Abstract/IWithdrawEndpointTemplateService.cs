using MongoDB.Bson;
using OnAim.Admin.APP.Services.WithdrawEndpointTemplate;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IWithdrawEndpointTemplateService
{
    Task<ApplicationResult> GetAllWithdrawEndpointTemplates(BaseFilter baseFilter);
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> CreateWithdrawEndpointTemplate(CreateWithdrawEndpointTemplateDto create);
    Task<ApplicationResult> UpdateWithdrawEndpointTemplate(UpdateWithdrawEndpointTemplateDto update);
}
