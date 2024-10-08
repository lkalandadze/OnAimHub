using GameLib.Application.Models.PrizeType;
using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class PrizeTypeController : BaseApiController
{
    private readonly IPrizeTypeService _prizeTypeService;

    public PrizeTypeController(IPrizeTypeService prizeTypeService)
    {
        _prizeTypeService = prizeTypeService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PrizeTypeGetModel>>> GetAllAsync()
    {
        return Ok(await _prizeTypeService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PrizeTypeGetModel>> GetByIdAsync(int id)
    {
        return Ok(await _prizeTypeService.GetByIdAsync(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateAsync([FromBody] PrizeTypeCreateModel model)
    {
        await _prizeTypeService.CreateAsync(model);
        return StatusCode(201);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] PrizeTypeUpdateModel model)
    {
        await _prizeTypeService.UpdateAsync(id, model);
        return StatusCode(200);
    }
}