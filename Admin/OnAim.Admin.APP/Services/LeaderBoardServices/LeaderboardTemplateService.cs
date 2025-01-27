using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.APP.Services.LeaderBoard;

public class LeaderboardTemplateService : ILeaderboardTemplateService
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;

    public LeaderboardTemplateService(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task<ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>> GetAllLeaderboardTemplates(BaseFilter filter)
    {
        var temps = await _leaderboardTemplateRepository.GetLeaderboardTemplates();

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    temps = temps.Where(u => u.IsDeleted == false).ToList();
                    break;
                case HistoryStatus.Deleted:
                    temps = temps.Where(u => u.IsDeleted == true).ToList();
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        var totalCount = temps.Count();

        var pageNumber = filter?.PageNumber ?? 1;
        var pageSize = filter?.PageSize ?? 25;

        var tempss = temps.Select(x => new LeaderBoardTemplateListDto
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            EventType = (Contracts.Dtos.LeaderBoard.EventType)x.EventType,
            CreationDate = x.CreationDate,
            AnnouncementDate = (x.AnnouncementDate).TotalMilliseconds,
            StartDate = (x.StartDate).TotalMilliseconds,
            EndDate = (x.EndDate).TotalMilliseconds,
            IsDeleted = x.IsDeleted,
            LeaderboardTemplatePrizes = x.LeaderboardTemplatePrizes.Select(xx => new leaderboardTemplatePrizesDto
            {
                Id = xx.Id,
                Amount = xx.Amount,
                CoinId = xx.CoinId,
                StartRank = xx.StartRank,
                EndRank = xx.EndRank
            }).ToList()
        });

        var res = tempss
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        return new ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>
        {
            Success = true,
            Data = new PaginatedResult<LeaderBoardTemplateListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res.ToList(),
            },
        };
    }

    public async Task<ApplicationResult<LeaderBoardTemplateListDto>> GetLeaderboardTemplateById(string id)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(id);

        if (template == null) throw new NotFoundException("template Not Found");

        var temp = new LeaderBoardTemplateListDto
        {
            Id = template.Id,
            Description = template.Description,
            Title = template.Title,
            EventType = (Contracts.Dtos.LeaderBoard.EventType)template.EventType,
            CreationDate = template.CreationDate,
            AnnouncementDate = (template.AnnouncementDate).TotalMilliseconds,
            StartDate = (template.StartDate).TotalMilliseconds,
            EndDate = (template.EndDate).TotalMilliseconds,
            IsDeleted = template.IsDeleted,
            LeaderboardTemplatePrizes = template.LeaderboardTemplatePrizes.Select(xx => new leaderboardTemplatePrizesDto
            {
                Id = xx.Id,
                Amount = xx.Amount,
                CoinId = xx.CoinId,
                StartRank = xx.StartRank,
                EndRank = xx.EndRank
            }).ToList()
        };

        return new ApplicationResult<LeaderBoardTemplateListDto> { Success = true, Data = temp };
    }

    public async Task<LeaderboardTemplate> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create)
    {
        var announcementDuration = TimeSpan.FromMilliseconds(create.AnnouncementDuration);
        var startDuration = TimeSpan.FromMilliseconds(create.StartDuration);
        var endDuration = TimeSpan.FromMilliseconds(create.EndDuration);

        var leaderboardTemplate = new LeaderboardTemplate(
            create.Title,
            create.Description,
            announcementDuration,
            startDuration,
            endDuration
        );

        foreach (var prize in create.LeaderboardPrizes)
        {
            leaderboardTemplate.AddLeaderboardTemplatePrizes(prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardTemplateRepository.AddLeaderboardTemplateAsync(leaderboardTemplate);

        return leaderboardTemplate;
    }

    public async Task<ApplicationResult<bool>> DeleteLeaderboardTemplate(string temp)
    {
        var template = await _leaderboardTemplateRepository.GetLeaderboardTemplateByIdAsync(temp);

        if (template == null)
            throw new NotFoundException("Template Not Found");

        template.Delete();

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(temp, template);

        return new ApplicationResult<bool> { Success = true };
    }

    public async Task<ApplicationResult<bool>> UpdateLeaderboardTemplate(UpdateLeaderboardTemplateDto update)
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
            template.UpdateLeaderboardPrizes(prize.Id.ToString(), prize.StartRank, prize.EndRank, prize.CoinId, prize.Amount);
        }

        await _leaderboardTemplateRepository.UpdateLeaderboardTemplateAsync(update.Id, template);

        return new ApplicationResult<bool> { Success = true };
    }
}