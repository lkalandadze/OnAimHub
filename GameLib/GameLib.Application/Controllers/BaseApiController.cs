using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{

}