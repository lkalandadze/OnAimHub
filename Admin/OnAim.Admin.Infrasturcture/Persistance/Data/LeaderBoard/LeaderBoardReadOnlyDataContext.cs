using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data.LeaderBoard;

public class LeaderBoardReadOnlyDataContext : DbContext
{
    public LeaderBoardReadOnlyDataContext(DbContextOptions<LeaderBoardReadOnlyDataContext> options) : base(options)
    {
        
    }

    public DbSet<LeaderboardRecord> LeaderboardRecords { get; set; }
    public DbSet<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; }
    public DbSet<LeaderboardProgress> LeaderboardProgresses { get; set; }
    public DbSet<LeaderboardResult> LeaderboardResults { get; set; }

    public IQueryable<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>().AsNoTracking();
    }
}
