using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.UserBondToRole;

public record UserBondToRoleCommand(int UserId, List<RoleDto>? Roles) : ICommand<ApplicationResult>;
