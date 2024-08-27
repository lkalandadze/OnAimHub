using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;
using System.Security.Claims;

namespace OnAim.Admin.API.Controllers.Abstract
{
    [Permission]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator
        {
            get
            {
                if (_mediator is null)
                    _mediator = HttpContext.RequestServices.GetService<IMediator>();

                return _mediator!;
            }
        }

        protected ClaimsPrincipal CurrentUser => User;

        protected string GetUserClaimValue(string claimType)
        {
            return User.FindFirstValue(claimType) ?? string.Empty;
        }

        protected string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
