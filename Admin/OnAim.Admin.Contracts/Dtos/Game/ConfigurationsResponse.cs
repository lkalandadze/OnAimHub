﻿using System.Text.Json.Serialization;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class ConfigurationsResponse
{
    [JsonPropertyName("succeeded")]
    public bool Succeeded { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }

    [JsonPropertyName("validationErrors")]
    public string ValidationErrors { get; set; }

    [JsonPropertyName("data")]
    public List<ConfigurationData> Data { get; set; }
}