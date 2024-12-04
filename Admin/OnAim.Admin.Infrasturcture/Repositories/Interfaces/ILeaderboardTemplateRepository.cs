using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Interfaces;

public interface ILeaderboardTemplateRepository
{
    Task AddLeaderboardTemplateAsync(LeaderboardTemplate template);
    Task<List<LeaderboardTemplate>> GetLeaderboardTemplates();
    Task<LeaderboardTemplate?> GetLeaderboardTemplateByIdAsync(string id);
    Task<LeaderboardTemplate?> UpdateLeaderboardTemplateAsync(string id, LeaderboardTemplate updated);
}
