#nullable disable

using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Wheel.Application.Models.Game;

public class GameResponseModel
{
    public string Name { get; set; }
    public DateTimeOffset ActivationTime { get; set; }
}