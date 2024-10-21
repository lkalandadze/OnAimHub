﻿#nullable disable

using Hub.Application.Models.Game;

namespace Hub.Application.Features.GameFeatures.Dtos;

public class GameBaseDtoModel
{
    public string Name { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public int ConfigurationCount { get; set; }
    public IEnumerable<string> Segments { get; set; }

    public static GameBaseDtoModel MapFrom(string name, GameShortInfoGetModel game)
    {
        return new GameBaseDtoModel
        {
            Name = name,
            Status = game.Status,
            Description = game.Description,
            ConfigurationCount = game.ConfigurationCount,
            Segments = game.Segments,
        };
    }
}