using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

using static OnAim.Admin.API.PolicyProvider.Policy.PermissionAuthorizeAttribute;

namespace OnAim.Admin.API.PolicyProvider.Policy
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return await base.GetPolicyAsync(policyName);
            }

            PermissionOperator @operator = GetOperatorFromPolicy(policyName);
            string[] permissions = GetPermissionsFromPolicy(policyName);

            var requirement = new PermissionRequirement(@operator, permissions);

            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}
