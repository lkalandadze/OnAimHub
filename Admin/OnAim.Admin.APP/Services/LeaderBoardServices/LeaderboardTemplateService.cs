using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderboardTemplateService : ILeaderboardTemplateService
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;

    public LeaderboardTemplateService(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task<ApplicationResult> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create)
    {
        var leaderboardTemplate = new LeaderboardTemplate(
            create.Title, 
            create.Description, 
            create.AnnouncementDate, 
            create.StartDate, 
            create.EndDate
            );

        foreach (var prize in create.LeaderboardPrizes)
        {
            leaderboardTemplate.AddLeaderboardTemplatePrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardTemplateRepository.AddLeaderboardTemplateAsync(leaderboardTemplate);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> GetAllLeaderboardTemplate()
    {
        var temps = await _leaderboardTemplateRepository.GetLeaderboardTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var coin = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("Coin Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<ApplicationResult> DeleteLeaderboardTemplate(string temp)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(temp);

        if (template == null)
        {
            throw new NotFoundException("Coin Template Not Found");
        }

        template.Delete();

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(temp, template);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdateCoinTemplate(UpdateLeaderboardTemplateDto update)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(update.Id);

        if (template == null)
        {
            throw new NotFoundException($"Coin template with the specified ID: [{update.Id}] was not found.");
        }

        template.Update(
            update.Name, 
            update.Description,
            (Domain.LeaderBoradEntities.EventType)update.EventType,
            update.AnnouncementDate,
            update.StartDate,
            update.EndDate
            );

        foreach (var prize in update.LeaderboardPrizes)
        {
            template.UpdateLeaderboardPrizes(prize.Id, prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(update.Id, template);

        return new ApplicationResult { Success = true };
    }
}