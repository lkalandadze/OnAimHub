﻿using AggregationService.Domain.Entities;

namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public sealed class CreateLeaderboardRecord
{
    public int PromotionId { get; set; }
    public string PromotionName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public int? ScheduleId { get; set; }
    public string? TemplateId { get; set; }
    public int? CreatedBy { get; set; }
    public Guid CorrelationId { get; set; }
    public List<CreateLeaderboardRecordPrizeCommandItem> LeaderboardPrizes { get; set; }
    public List<AggregationConfiguration>? AggregationConfigurations { get; set; }
}
