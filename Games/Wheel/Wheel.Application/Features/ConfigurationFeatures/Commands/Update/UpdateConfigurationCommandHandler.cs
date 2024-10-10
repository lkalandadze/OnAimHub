using GameLib.Application.Generators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;
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

                UpdateEntity(existingConfiguration, updatedConfiguration);

                _wheelConfigurationRepository.Update(existingConfiguration);
                await _wheelConfigurationRepository.SaveAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateEntity(WheelConfiguration existingEntity, WheelConfiguration updatedEntity)
        {
            var properties = typeof(WheelConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (IsCollection(property.PropertyType))
                {
                    var existingCollection = property.GetValue(existingEntity) as IEnumerable;
                    var updatedCollection = property.GetValue(updatedEntity) as IEnumerable;

                    UpdateCollection(existingCollection, updatedCollection, property.PropertyType);
                }
                else
                {
                    var updatedValue = property.GetValue(updatedEntity);
                    if (updatedValue != null)
                    {
                        property.SetValue(existingEntity, updatedValue);
                    }
                }
            }
        }

private void UpdateCollection(IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType)
{
    if (existingCollection == null || updatedCollection == null) return;

    var dbContext = _wheelConfigurationRepository.GetDbContext(); // Get the current DbContext
    var itemType = collectionType.GetGenericArguments().FirstOrDefault();
    if (itemType == null) return;

    // Clear the existing collection using reflection
    var clearMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Clear");
    clearMethod?.Invoke(existingCollection, null);

    // Add the updated items to the existing collection
    var addMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Add");

    foreach (var updatedItem in updatedCollection)
    {
        var entityEntry = dbContext.Entry(updatedItem);

        if (entityEntry.State == EntityState.Detached)
        {
            dbContext.Attach(updatedItem); // Attach the entity if it's not already tracked
        }

        entityEntry.State = EntityState.Modified; // Mark the entity as modified

        addMethod?.Invoke(existingCollection, new[] { updatedItem });
    }
}


        private bool IsCollection(Type type)
        {
            return type.IsGenericType && (typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()) || typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }
    }
}
