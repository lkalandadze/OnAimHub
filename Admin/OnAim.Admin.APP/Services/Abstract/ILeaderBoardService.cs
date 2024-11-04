using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ILeaderBoardService
{
    Task<ApplicationResult> GetLeaderBoardTemplates(BaseFilter filter);
    Task<ApplicationResult> GetAllLeaderBoard(LeaderBoardFilter? filter);
    Task<ApplicationResult> GetAllPrizes();
    Task<ApplicationResult> CreateTemplate(CreateLeaderboardTemplateDto createLeaderboardTemplateDto);
    Task<ApplicationResult> UpdateTemplate(UpdateLeaderboardTemplateDto updateLeaderboardTemplateDto);
    Task<ApplicationResult> CreateLeaderBoardRecord(CreateLeaderboardRecordDto createLeaderboardRecordDto);
    Task<ApplicationResult> UpdateLeaderBoardRecord(UpdateLeaderboardRecordDto updateLeaderboardRecordDto);
    Task<ApplicationResult> Schedule(int templateId);
    Task<ApplicationResult> Execute(int templateId);
}
