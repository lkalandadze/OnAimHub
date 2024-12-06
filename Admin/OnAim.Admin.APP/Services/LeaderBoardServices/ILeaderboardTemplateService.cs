using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderboardTemplateService
{
    Task<LeaderboardTemplate> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create);
    Task<ApplicationResult> GetAllLeaderboardTemplate();
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateLeaderboardTemplateDto update);
}
