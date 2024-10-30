using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public record CreateEmailDomainCommand(List<DomainDto>? Domains, string Domain, bool? IsActive) : ICommand<ApplicationResult>;
