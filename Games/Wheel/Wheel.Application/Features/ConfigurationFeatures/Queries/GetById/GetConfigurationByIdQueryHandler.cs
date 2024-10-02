using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Domain.Generators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;

public class GetConfigurationByIdQueryHandler : IRequestHandler<GetConfigurationByIdQuery, GetConfigurationByIdQueryResponse>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly EntityGenerator _entityGenerator;

    public GetConfigurationByIdQueryHandler(IConfigurationRepository configurationRepository, EntityGenerator entityGenerator)
    {
        _configurationRepository = configurationRepository;
        _entityGenerator = entityGenerator;
    }

    public async Task<GetConfigurationByIdQueryResponse> Handle(GetConfigurationByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var configuration = await _configurationRepository
                .Query()
                .Include(x => x.Prices)
                .Include(x => x.Segments)
                .FirstOrDefaultAsync(x => x.Id == request.ConfigurationId);

            if (configuration == null)
            {
                // Throw a custom exception or return a not-found response
                throw new KeyNotFoundException($"Configuration with ID {request.ConfigurationId} was not found.");
            }

            var metadata = _entityGenerator.GenerateEntityMetadata(typeof(Configuration));

            return new GetConfigurationByIdQueryResponse
            {
                Metadata = metadata,
                ConfigurationData = configuration
            };
        }
        catch (Exception ex)
        {
            // Log and re-throw or handle the exception
            Console.WriteLine($"Error occurred: {ex.Message}");
            throw;
        }
    }
}