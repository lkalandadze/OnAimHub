using System.Security.Claims;

namespace OnAim.Admin.APP.Services.Admin.AuthServices.Auth;

public class SecurityContextAccessor : ISecurityContextAccessor
{
    private readonly ILogger<SecurityContextAccessor> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SecurityContextAccessor(IHttpContextAccessor httpContextAccessor,
        ILogger<SecurityContextAccessor> logger)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(user, out int userId))
            {
                return userId;
            }

            return userId;
        }
    }

    public string JwtToken
    {
        get
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            var isAuthenticated = _httpContextAccessor.HttpContext?.User?.Identities?.FirstOrDefault()?.IsAuthenticated;
            return isAuthenticated.HasValue && isAuthenticated.Value;
        }
    }

    public string Role
    {
        get
        {
            var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            return role;
        }
    }
}
