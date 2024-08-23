using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnAim.Admin.Identity.Entities;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Shared.Configuration;
using System.Reflection;
using System.Security.Claims;

namespace OnAim.Admin.Identity
{
    public static class Extensions
    {
        public static IServiceCollection AddIdentityServerAuthentication<TContext, TUser, TUserManager>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : IdentityDbContext<TUser>
            where TUser : IdentityUser
            where TUserManager : UserManager<TUser>
        {
            var authenticationConfig = new AuthenticationConfig();
            configuration.GetSection(AuthenticationConfig.ToString()).Bind(authenticationConfig);

            //var connectionString = configuration.GetSection("DefaultConnectionString").Value;
            var connectionString = "Host=localhost;Port=5432;Database=OnAimAdmin;Username=postgres;Password=12345678;Include Error Detail=true";

            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddIdentity<TUser, IdentityRole>(options =>
            {
                options.Lockout = new LockoutOptions();
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddClaimsPrincipalFactory<MapClaieTypes>()
            .AddEntityFrameworkStores<TContext>()
            .AddUserManager<TUserManager>()
            .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(MigrationAssembly).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<JwtBearerOptions>(options =>
            {
                //options.RequireHttpsMetadata = false;
                options.MapInboundClaims = false;
                options.Authority = authenticationConfig.Authority;
                options.Audience = "on-aim";
                options.TokenValidationParameters = new TokenValidationParameters();
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                options.ForwardDefaultSelector = (HttpContext context) =>
                {
                    var (scheme, credential) = GetSchemeAndCredential(context);
                    if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) &&
                        !credential.Contains('.'))
                    {
                        return "introspection";
                    }

                    return null;
                };
            });

            //services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //    {
            //        options.RequireHttpsMetadata = false;
            //        options.MapInboundClaims = false;
            //        options.Authority = authenticationConfig.Authority;
            //        options.Audience = "on-aim";
            //        options.TokenValidationParameters = new TokenValidationParameters();
            //        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            //        options.ForwardDefaultSelector = (HttpContext context) =>
            //        {
            //            var (scheme, credential) = GetSchemeAndCredential(context);
            //            if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) &&
            //                !credential.Contains('.'))
            //            {
            //                return "introspection";
            //            }

            //            return null;
            //        };
            //    })
            //    .AddOAuth2Introspection("introspection", options =>
            //    {
            //        options.Authority = authenticationConfig.Authority;

            //        options.ClientId = "on-aim";
            //        options.ClientSecret = authenticationConfig.ClientSecret;
            //    });

            services.AddAccessTokenManagement();

            return services;
        }

        private static (string, string) GetSchemeAndCredential(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                return ("", "");
            }

            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return ("", "");
            }

            return (parts[0], parts[1]);
        }
    }
    public class MapClaieTypes : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public MapClaieTypes(
            ApplicationUserManager userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            if (principal.Identity != null && principal.Identity is ClaimsIdentity identity)
            {
                var jwtRoleClaims = identity.FindAll(JwtClaimTypes.Role);
                identity.AddClaims(jwtRoleClaims.Select(x => new Claim(ClaimTypes.Role, x.Value)).ToList());

                var jwtNameClaims = identity.FindAll(JwtClaimTypes.Name);
                identity.AddClaims(jwtNameClaims.Select(x => new Claim(ClaimTypes.NameIdentifier, x.Value)).ToList());
            }

            return principal;
        }
    }

    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement where TAttribute : Attribute
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var attributes = new List<TAttribute>();

            if ((context.Resource as AuthorizationFilterContext)?.ActionDescriptor is ControllerActionDescriptor action)
            {
                attributes.AddRange(GetAttributes(action.ControllerTypeInfo.UnderlyingSystemType));
                attributes.AddRange(GetAttributes(action.MethodInfo));
            }

            return HandleRequirementAsync(context, requirement, attributes);
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement,
            IEnumerable<TAttribute> attributes);

        private static IEnumerable<TAttribute> GetAttributes(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }
    }

    public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement,
            IEnumerable<PermissionAttribute> attributes)
        {
            foreach (var permissionAttribute in attributes)
            {
                if (!await AuthorizeAsync(context.User, permissionAttribute.ClaimType, permissionAttribute.ClaimsRequirement))
                {
                    return;
                }
            }

            context.Succeed(requirement);
        }

        private Task<bool> AuthorizeAsync(ClaimsPrincipal user, string claimType, string[] permissions)
        {
            return Task.FromResult(!permissions
                .Except(user.Claims.ToList().Where(c => c.Type == claimType).Select(c => c.Value)).Any());
        }
    }

    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public string[] ClaimsRequirement { get; }
        public string ClaimType { get; set; }


        public PermissionAttribute(string claimType = null, params string[] claimsRequirement) : base("BasePolicy")
        {
            ClaimsRequirement = claimsRequirement;
            ClaimType = claimType;
        }
    }
}
