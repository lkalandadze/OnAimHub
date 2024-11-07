using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Commands.UpdatePrizeType;

public record UpdatePrizeTypeCommand(int Id, CreatePrizeTypeDto CreatePrizeType) : ICommand<ApplicationResult>;
