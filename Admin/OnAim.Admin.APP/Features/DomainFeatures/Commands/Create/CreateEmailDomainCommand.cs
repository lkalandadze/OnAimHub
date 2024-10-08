using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public record CreateEmailDomainCommand(List<DomainDto>? Domains, string Domain, bool? IsActive) : ICommand<ApplicationResult>;
