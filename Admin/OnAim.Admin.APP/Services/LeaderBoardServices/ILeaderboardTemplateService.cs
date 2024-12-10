using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderboardTemplateService
{
    Task<LeaderboardTemplate> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create);
    Task<ApplicationResult> GetAllLeaderboardTemplates(BaseFilter filter);
    Task<ApplicationResult> GetLeaderboardTemplateById(string id);
    Task<ApplicationResult> UpdateLeaderboardTemplate(UpdateLeaderboardTemplateDto update);
    Task<ApplicationResult> DeleteLeaderboardTemplate(string temp);
}
