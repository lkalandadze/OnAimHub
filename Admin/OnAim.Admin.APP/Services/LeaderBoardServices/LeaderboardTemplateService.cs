﻿using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderboardTemplateService : ILeaderboardTemplateService
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;

    public LeaderboardTemplateService(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task<ApplicationResult> GetAllLeaderboardTemplates(BaseFilter filter)
    {
        var temps = await _leaderboardTemplateRepository.GetLeaderboardTemplates();

        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var res = temps
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<LeaderboardTemplate>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult> GetLeaderboardTemplateById(string id)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(id);

        if (template == null) throw new NotFoundException("template Not Found");

        return new ApplicationResult { Success = true, Data = template };
    }

    public async Task<LeaderboardTemplate> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create)
    {
        var leaderboardTemplate = new LeaderboardTemplate(
            create.Title,
            create.Description,
            create.AnnouncementDuration,
            create.StartDuration,
            create.EndDuration
            );

        foreach (var prize in create.LeaderboardPrizes)
        {
            leaderboardTemplate.AddLeaderboardTemplatePrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardTemplateRepository.AddLeaderboardTemplateAsync(leaderboardTemplate);

        return leaderboardTemplate;
    }

    public async Task<ApplicationResult> DeleteLeaderboardTemplate(string temp)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(temp);

        if (template == null)
            throw new NotFoundException("Template Not Found");

        template.Delete();

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(temp, template);

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> UpdateLeaderboardTemplate(UpdateLeaderboardTemplateDto update)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(update.Id);

        if (template == null)
            throw new NotFoundException($"template with the specified ID: [{update.Id}] was not found.");

        template.Update(
            update.Name,
            update.Description,
            (Domain.LeaderBoradEntities.EventType)update.EventType,
            update.AnnouncementDuration,
            update.StartDuration,
            update.EndDuration
            );

        foreach (var prize in update.LeaderboardPrizes)
        {
            template.UpdateLeaderboardPrizes(prize.Id, prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(update.Id, template);

        return new ApplicationResult { Success = true };
    }
}