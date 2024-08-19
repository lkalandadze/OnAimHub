using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.API.Factory
{
    public class ApplicationContext
    {
        public string ClientId { get; init; }
        public int? UserId { get; init; }
        public ICollection<UserRole> UserRoles { get; private set; }
        public UserData? UserData { get; }
        public string? ClientIpAddress { get; private set; }

        public ApplicationContext(string? clientIpAddress)
        {
            ClientIpAddress = clientIpAddress;
        }

        public ApplicationContext(string clientId, UserData? userData, string? clientIpAddress)
        {
            ClientId = clientId;
            UserData = userData;
            ClientIpAddress = clientIpAddress;
        }

        public ApplicationContext(string clientId, int userId, ICollection<UserRole> roles, UserData? userData, string? clientIpAddress)
        {
            ClientId = clientId;
            UserId = userId;
            UserRoles = roles;
            UserData = userData;
            ClientIpAddress = clientIpAddress;
        }
    }
}
