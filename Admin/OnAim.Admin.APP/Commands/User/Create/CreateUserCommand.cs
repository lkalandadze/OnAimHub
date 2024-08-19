using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class CreateUserCommand : IRequest<ApplicationResult>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //public List<RoleModel>? Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int? UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
