using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public record BlockSegmentForPlayersCommand(string SegmentId, IFormFile File) : ICommand<ApplicationResult>;
