using MongoDB.Bson;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.LeaderBoard;

namespace OnAim.Admin.APP.Services.Abstract;

public interface ILeaderboardTemplateService
{
    Task<ApplicationResult> CreateLeaderboardTemplate(CreateLeaderboardTemplateDto create);
    Task<ApplicationResult> GetAllLeaderboardTemplate();
    Task<ApplicationResult> GetById(ObjectId id);
    Task<ApplicationResult> UpdateCoinTemplate(UpdateLeaderboardTemplateDto update);
}
