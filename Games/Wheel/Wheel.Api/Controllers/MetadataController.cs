using GameLib.Application.Controllers;
using GameLib.Application.Generators;
using GameLib.Domain.Abstractions.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.DataAccess;

namespace Wheel.Api.Controllers;

public class MetadataController : BaseApiController
{

    private readonly EntityGenerator _entityGenerator;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly WheelConfigDbContext _context;
    

    public MetadataController(EntityGenerator entityGenerator, IConfigurationRepository configurationRepository, WheelConfigDbContext context)
    {
        _entityGenerator = entityGenerator;
        _configurationRepository = configurationRepository;
        _context = context;
    }
    [HttpGet("TestInclude")]
    public async Task<WheelConfiguration> GetConfigurationWithIncludes(int configurationId)
    {
        var query = _context.Set<WheelConfiguration>().AsQueryable();
        query = query.IncludeNotHiddenAll();

        var configuration = await query.FirstOrDefaultAsync(c => c.Id == configurationId);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration with ID {configurationId} was not found.");
        }

        return configuration;
    }

    [HttpGet("Test/{id}")]
    public async Task<IActionResult> GetConfiguration(int id)
    {
        try
        {
            var configuration = GetConfigurationWithIncludes(id);
            return Ok(configuration);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("configuration")]
    public IActionResult GetConfigurationMetadata()
    {
        var metadata = _entityGenerator.GenerateEntityMetadata(typeof(WheelConfiguration));
        return Ok(metadata);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetConfigurationByIdQueryResponse>> GetConfigurationById(int id)
    {
        var query = new GetConfigurationByIdQuery { ConfigurationId = id };
        try
        {
            return await Mediator.Send(query);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
