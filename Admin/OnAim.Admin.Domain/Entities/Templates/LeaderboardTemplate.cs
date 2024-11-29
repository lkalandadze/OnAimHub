using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class LeaderboardTemplate
{
    public LeaderboardTemplate()
    {
        
    }
    public LeaderboardTemplate(string name, string description, System.TimeSpan startTime, int announceIn, int startIn, int endIn)
    {
        Name = name;
        Description = description;
        StartTime = startTime;
        AnnounceIn = announceIn;
        StartIn = startIn;
        EndIn = endIn;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public System.TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public ICollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; set; } = new List<LeaderboardTemplatePrize>();
    public ICollection<LeaderboardRecord> LeaderboardRecords { get; set; } = new List<LeaderboardRecord>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>();

    public void AddLeaderboardTemplatePrizes(int startRank, int endRank, string prizeId, int amount)
    {
        var prize = new LeaderboardTemplatePrize(startRank, endRank, prizeId, amount);
        LeaderboardTemplatePrizes.Add(prize);
    }

    public void UpdateLeaderboardPrizes(int? id, int startRank, int endRank, string prizeId, int amount)
    {
        if (id.HasValue)
        {
            var existingPrize = LeaderboardTemplatePrizes.FirstOrDefault(x => x.Id == id.Value);
            if (existingPrize != null)
            {
                existingPrize.Update(startRank, endRank, prizeId, amount);
                return;
            }
        }

        var newPrize = new LeaderboardTemplatePrize(startRank, endRank, prizeId, amount);
        LeaderboardTemplatePrizes.Add(newPrize);
    }

    public void Update(string name, string description, System.TimeSpan startTime, int announceIn, int startIn, int endIn)
    {
        Name = name;
        Description = description;
        StartTime = startTime;
        AnnounceIn = announceIn;
        StartIn = startIn;
        EndIn = endIn;
    }
}
