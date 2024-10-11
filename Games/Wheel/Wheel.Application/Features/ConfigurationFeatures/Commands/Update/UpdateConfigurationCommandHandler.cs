using GameLib.Application.Generators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;

namespace Wheel.Application.Features.ConfigurationFeatures.Commands.Update
{
    public class UpdateConfigurationCommandHandler : IRequestHandler<UpdateConfigurationCommand>
    {
        private readonly IWheelConfigurationRepository _wheelConfigurationRepository;

        public UpdateConfigurationCommandHandler(IWheelConfigurationRepository wheelConfigurationRepository)
        {
            _wheelConfigurationRepository = wheelConfigurationRepository;
        }

        public async Task Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updatedConfiguration = JsonConvert.DeserializeObject<WheelConfiguration>(request.ConfigurationJson);

                if (updatedConfiguration == null)
                    throw new ArgumentException("Invalid JSON data for configuration update.");

                // Get existing configuration
                var existingConfiguration = await _wheelConfigurationRepository.Query()
                    .IncludeNotHiddenAll(typeof(WheelConfiguration))
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == updatedConfiguration.Id, cancellationToken);

                if (existingConfiguration == null)
                    throw new Exception("Configuration not found");

                _wheelConfigurationRepository.UpdateEntity(existingConfiguration, updatedConfiguration);


                _wheelConfigurationRepository.Update(existingConfiguration);
                await _wheelConfigurationRepository.SaveAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
