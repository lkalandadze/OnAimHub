using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public record UserProfileUpdateCommand(int Id, ProfileUpdateRequest profileUpdateRequest) : ICommand<ApplicationResult>;

public record ProfileUpdateRequest(string? FirstName, string? LastName, string? Phone);
