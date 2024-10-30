using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Commands.Schedule;

public record CreateScheduleCommand(int TemplateId) : ICommand<ApplicationResult>;
