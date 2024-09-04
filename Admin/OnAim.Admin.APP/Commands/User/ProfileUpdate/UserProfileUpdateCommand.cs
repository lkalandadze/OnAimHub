using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.ProfileUpdate
{
    public record UserProfileUpdateCommand(int Id, ProfileUpdateRequest profileUpdateRequest) : ICommand<ApplicationResult>;

    public record ProfileUpdateRequest(string? FirstName, string? LastName, string? Phone);
}
