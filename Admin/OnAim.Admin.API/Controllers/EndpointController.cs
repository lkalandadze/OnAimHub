using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Attributes;
using OnAim.Admin.API.Service;
using OnAim.Admin.API.Service.Endpoint;
using OnAim.Admin.APP.Commands.EndPoint.Create;
using OnAim.Admin.APP.Commands.EndPoint.Disable;
using OnAim.Admin.APP.Commands.EndPoint.Enable;
using OnAim.Admin.APP.Models;
using OnAim.Admin.APP.Queries.EndPoint.GetAll;
using OnAim.Admin.APP.Queries.EndPoint.GetById;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using System.Net;

namespace OnAim.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndpointController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEndpointService _endpointService;

        public EndpointController(IMediator mediator, IEndpointService endpointService)
        {
            _mediator = mediator;
            _endpointService = endpointService;
        }

        [HttpGet("all")]
        public IActionResult GetAllEndpoints()
        {
            var endpoints = _endpointService.GetAllEndpoints();
            return Ok(endpoints);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _endpointService.SaveEndpointsAsync();
            return Ok();
        }

        [HttpPost("Create")]
        [Permission("Endpoint/Create")]
        [ProducesResponseType(typeof(CreateEndpointCommand), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEndpointCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission("Endpoint/Enable")]
        [HttpPut("Enable/{endpointId}")]
        public async Task<IActionResult> Enable([FromRoute] string endpointId)
        {
            return Ok(await _mediator.Send(new EnableEndpointCommand(endpointId)));
        }

        [Permission("Endpoint/Disable")]
        [HttpPut("Disable/{endpointId}")]
        public async Task<IActionResult> Disable([FromRoute] string endpointId)
        {
            return Ok(await _mediator.Send(new DisableEndpointCommand(endpointId)));
        }

        [Permission("Endpoint/GetAll")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RoleFilter roleFilter)
        {
            return Ok(await _mediator.Send(new GetAllEndpointQuery(roleFilter)));
        }

        [Permission("Endpoint/Get")]
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _mediator.Send(new GetEndpointByIdQuery(id)));
        }
    }
}
