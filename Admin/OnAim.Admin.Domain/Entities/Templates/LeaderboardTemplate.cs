using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnAim.Admin.Domain.LeaderBoradEntities;

namespace OnAim.Admin.Domain.Entities.Templates;

public class LeaderboardTemplate
{
    public LeaderboardTemplate(){}

    public LeaderboardTemplate(
        string title, 
        string description,
        DateTimeOffset announcementDate,
        DateTimeOffset startDate,
        DateTimeOffset endDate)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Title = title;
        Description = description;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        CreationDate = DateTimeOffset.UtcNow;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public bool IsDeleted { get; set; } 
    public DateTimeOffset DateDeleted { get; set; }

    public ICollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; set; } = new List<LeaderboardTemplatePrize>();

    public void AddLeaderboardTemplatePrizes(int startRank, int endRank, string coinId, int amount)
    {
        var prize = new LeaderboardTemplatePrize(startRank, endRank, coinId, amount);
        LeaderboardTemplatePrizes.Add(prize);
    }

    public void UpdateLeaderboardPrizes(string? id, int startRank, int endRank, string coinId, int amount)
    {
        if (id != null)
        {
            var existingPrize = LeaderboardTemplatePrizes.FirstOrDefault(x => x.Id == id);
            if (existingPrize != null)
            {
                existingPrize.Update(startRank, endRank, coinId, amount);
                return;
            }
        }

        var newPrize = new LeaderboardTemplatePrize(startRank, endRank, coinId, amount);
        LeaderboardTemplatePrizes.Add(newPrize);
    }

    public void Update(
        string title, 
        string description,
        EventType eventType,
        DateTimeOffset announcementDate,
        DateTimeOffset startDate,
        DateTimeOffset endDate)
    {
        Title = title;
        Description = description;
        EventType = eventType;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
