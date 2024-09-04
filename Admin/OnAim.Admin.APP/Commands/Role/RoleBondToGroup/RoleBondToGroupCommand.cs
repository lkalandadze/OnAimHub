using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.RoleBondToGroup
{
    public record RoleBondToGroupCommand(int RoleId, List<GroupDto>? Groups, List<UserDto>? Users) : ICommand<ApplicationResult>;
    public record GroupDto(int Id, bool IsActive);
    public record UserDto(int Id, bool IsActive);
}
