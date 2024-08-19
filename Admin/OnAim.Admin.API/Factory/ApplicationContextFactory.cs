using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Security.Claims;
using System.Text.Json;

namespace OnAim.Admin.API.Factory
{
    public class ApplicationContextFactory
    {
        public static ApplicationContext Create(IServiceProvider ctx)
        {
            var httpContext = ctx.GetRequiredService<IHttpContextAccessor>().HttpContext;

            return Create(httpContext,
                          ctx.GetRequiredService<ApplicationUserManager>(),
                          ctx.GetRequiredService<IRepository<User>>());
        }

        public static ApplicationContext Create(HttpContext? httpContext,
                                                            ApplicationUserManager userManager,
                                                            IRepository<User> userRepository)
        {
            var remoteIpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();

            if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                var clientId = httpContext.User.FindFirstValue(IdentityModel.JwtClaimTypes.ClientId);
                if (!string.IsNullOrEmpty(clientId))
                {
                    var identityUser = userManager.GetUserAsync(httpContext.User).GetAwaiter().GetResult();
                    if (identityUser != null)
                    {
                        var user = userRepository.Query(x => x.Username == identityUser.UserName).FirstOrDefault();
                        var userClaims = userManager.GetClaimsAsync(identityUser).GetAwaiter().GetResult();
                        var userData = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);

                        return new ApplicationContext(clientId, user.Id, user.UserRoles, JsonSerializer.Deserialize<UserData>(userData.Value), remoteIpAddress);
                    }
                    else
                    {

                    }
                }
            }

            return new ApplicationContext(remoteIpAddress);
        }
    }
}
