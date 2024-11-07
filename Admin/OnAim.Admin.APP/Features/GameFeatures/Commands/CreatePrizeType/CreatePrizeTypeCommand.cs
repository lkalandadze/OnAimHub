using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.CreatePrizeType;

public record CreatePrizeTypeCommand(CreatePrizeTypeDto CreatePrizeType) : ICommand<ApplicationResult>;
