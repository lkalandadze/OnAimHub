using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Models.Response.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public sealed record UpdateUserCommand(int Id, UpdateUserDto Model) : IRequest<ApplicationResult>;
    public class UpdateUserDto
    {
        public List<RoleResponseModel> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
