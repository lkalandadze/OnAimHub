using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public record UserProfileUpdateCommand(int Id, ProfileUpdateRequest ProfileUpdateRequest) : ICommand<ApplicationResult>;

public record ProfileUpdateRequest(string? FirstName, string? LastName, string? Phone, bool? IsActive);
