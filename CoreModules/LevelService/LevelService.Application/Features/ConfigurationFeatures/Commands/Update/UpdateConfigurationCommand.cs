using MediatR;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Update;

public sealed class UpdateConfigurationCommand : IRequest
{
    public int StageId { get; set; }
    public int ConfigurationId { get; set; }
    public string CurrencyId { get; set; }
    public decimal ExperienceToGrant { get; set; }
}