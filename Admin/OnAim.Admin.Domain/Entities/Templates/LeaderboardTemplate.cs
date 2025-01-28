using AggregationService.Domain.Entities;
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
        TimeSpan announcementDate,
        TimeSpan startDate,
        TimeSpan endDate)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Title = title;
        Description = description;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        CreationDate = DateTime.UtcNow;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTime CreationDate { get; set; }
    public TimeSpan AnnouncementDate { get; set; }
    public TimeSpan StartDate { get; set; }
    public TimeSpan EndDate { get; set; }
    public bool IsDeleted { get; set; } 
    public DateTimeOffset DateDeleted { get; set; }
    public int Usage { get; set; }

    public ICollection<AggregationConfiguration> AggregationConfigurations { get; set; } = new List<AggregationConfiguration>();
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
        TimeSpan announcementDate,
        TimeSpan startDate,
        TimeSpan endDate)
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
