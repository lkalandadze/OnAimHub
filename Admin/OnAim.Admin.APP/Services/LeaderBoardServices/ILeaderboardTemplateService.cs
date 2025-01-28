using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.LeaderBoardServices;

public interface ILeaderboardTemplateService
{
    Task<LeaderboardTemplate> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create);
    Task<ApplicationResult<PaginatedResult<LeaderBoardTemplateListDto>>> GetAllLeaderboardTemplates(BaseFilter filter);
    Task<ApplicationResult<LeaderBoardTemplateListDto>> GetLeaderboardTemplateById(string id);
    Task<ApplicationResult<bool>> UpdateLeaderboardTemplate(UpdateLeaderboardTemplateDto update);
    Task<ApplicationResult<bool>> DeleteLeaderboardTemplate(string temp);
}
