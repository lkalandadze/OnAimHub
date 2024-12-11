using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;

namespace OnAim.Admin.API.Controllers.Abstract;

//[CheckEndpointStatusAttribute]
//[Permission]
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
}