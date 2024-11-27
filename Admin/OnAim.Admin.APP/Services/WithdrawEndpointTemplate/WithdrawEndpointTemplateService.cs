using MongoDB.Bson;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Infrasturcture.Repositories.Abstract;

namespace OnAim.Admin.APP.Services.WithdrawEndpointTemplate;

public class WithdrawEndpointTemplateService : IWithdrawEndpointTemplateService
{
    private readonly IWithdrawEndpointTemplateRepository _withdrawEndpointTemplateRepository;

    public WithdrawEndpointTemplateService(IWithdrawEndpointTemplateRepository withdrawEndpointTemplateRepository)
    {
        _withdrawEndpointTemplateRepository = withdrawEndpointTemplateRepository;
    }

    public async Task<ApplicationResult> GetAllWithdrawEndpointTemplates(BaseFilter baseFilter)
    {
        var temps = await _withdrawEndpointTemplateRepository.GetWithdrawEndpointTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(ObjectId id)
    {
        var coin = await _withdrawEndpointTemplateRepository.GetWithdrawEndpointTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> CreateWithdrawEndpointTemplate(CreateWithdrawEndpointTemplateDto create)
    {
        var template = new Admin.Domain.Entities.Templates.WithdrawEndpointTemplate 
        {
            Name = create.Name,
            Endpoint = create.Endpoint,
            ContentType = (Admin.Domain.EndpointContentType)create.ContentType,
            EndpointContent = create.Content
        };

        await _withdrawEndpointTemplateRepository.AddWithdrawEndpointTemplateAsync(template);

        return new ApplicationResult();
    }

    public async Task<ApplicationResult> UpdateWithdrawEndpointTemplate(UpdateWithdrawEndpointTemplateDto update)
    {
        var template = await _withdrawEndpointTemplateRepository.GetWithdrawEndpointTemplateByIdAsync(update.Id);

        if (template == null)
        {
            throw new Exception($"Endpoint template with the specified ID: [{update.Id}] was not found.");
        }

        template.Update(update.Name, update.Endpoint, update.Content, (Admin.Domain.EndpointContentType)update.ContentType);

        await _withdrawEndpointTemplateRepository.UpdateWithdrawEndpointTemplateAsync(update.Id, template);

        return new ApplicationResult { Success = true };
    }
}
public record CreateWithdrawEndpointTemplateDto(string Name, string Endpoint, string Content, EndpointContentType ContentType);
public record UpdateWithdrawEndpointTemplateDto(ObjectId Id, string Name, string Endpoint, string Content, EndpointContentType ContentType);
