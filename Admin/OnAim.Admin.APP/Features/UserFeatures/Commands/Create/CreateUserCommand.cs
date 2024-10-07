using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.ComponentModel.DataAnnotations;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Create;

public class CreateUserCommand : ICommand<ApplicationResult>
{
    [EmailAddress]
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}
