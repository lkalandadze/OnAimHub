using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public record BlockSegmentForPlayersCommand(IEnumerable<string> SegmentId, IFormFile File) : ICommand<ApplicationResult>;
