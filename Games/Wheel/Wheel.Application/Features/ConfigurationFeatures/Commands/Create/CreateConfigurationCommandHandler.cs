using GameLib.Application.Generators;
using GameLib.Domain.Abstractions;
using MediatR;
using Newtonsoft.Json;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;

namespace Wheel.Application.Features.ConfigurationFeatures.Commands.Create;

public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand>
{
    private readonly CommandGenerator _commandGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWheelConfigurationRepository _wheelConfigurationRepository;
    public CreateConfigurationCommandHandler(CommandGenerator commandGenerator, IUnitOfWork unitOfWork, IWheelConfigurationRepository wheelConfigurationRepository)
    {
        _commandGenerator = commandGenerator;
        _unitOfWork = unitOfWork;
        _wheelConfigurationRepository = wheelConfigurationRepository;
    }

    public async Task Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        string hardcodedJson = @"
        {
          'Name': 'Hardcoded Wheel Configuration',
          'Value': 1000,
          'IsActive': true,
          'Rounds': [
            {
              'Name': 'Hardcoded Round 1'
            },
            {
              'Name': 'Hardcoded Round 2'
            }
          ],
          'Prices': [
            {
              'Id': 'Price5',   // Set an explicit Id for each Price
              'Value': 200.50,
              'Multiplier': 1.5,
              'CurrencyId': 'OnAimCoin'
            },
            {
              'Id': 'Price6',   // Set an explicit Id for each Price
              'Value': 350.75,
              'Multiplier': 2.0,
              'CurrencyId': 'OnAimCoin'
            }
          ],
          'Segments': [
            {
              'Id': 'Segment5',   // Set an explicit Id for each Segment
              'IsDeleted': true
            },
            {
              'Id': 'Segment6',   // Set an explicit Id for each Segment
              'IsDeleted': false
            }
          ]
        }";
        var c = JsonConvert.DeserializeObject<WheelConfiguration>(hardcodedJson);
        await _wheelConfigurationRepository.InsertAsync(c);
        await _wheelConfigurationRepository.SaveAsync();

    }

    //// This method will dynamically set properties on the entity based on JSON data
    //private void PopulateEntityFromJson(object entity, JObject data)
    //{
    //    try
    //    {
    //        var entityType = entity.GetType();

    //        foreach (var prop in data.Properties())
    //        {
    //            // Find the property in the entity that matches the JSON field
    //            var propertyInfo = entityType.GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

    //            if (propertyInfo != null && propertyInfo.CanWrite)
    //            {
    //                // If the property is a collection (like Rounds, Prices, Segments)
    //                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) && propertyInfo.PropertyType != typeof(string))
    //                {
    //                    var listType = propertyInfo.PropertyType.GetGenericArguments().First();
    //                    var list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));

    //                    foreach (var item in (JArray)prop.Value)
    //                    {
    //                        var itemInstance = Activator.CreateInstance(listType);
    //                        PopulateEntityFromJson(itemInstance, (JObject)item); // Recursively populate the nested entity

    //                        // Check if the entity is of type Price or Segment and ensure Id is set
    //                        if (itemInstance is Price price && string.IsNullOrEmpty(price.Id))
    //                        {
    //                            price.Id = Guid.NewGuid().ToString();  // Generate a new Id for Price
    //                        }
    //                        else if (itemInstance is Segment segment && string.IsNullOrEmpty(segment.Id))
    //                        {
    //                            segment.Id = Guid.NewGuid().ToString();  // Generate a new Id for Segment
    //                        }

    //                        list.Add(itemInstance);
    //                    }

    //                    propertyInfo.SetValue(entity, list);
    //                }
    //                else
    //                {
    //                    // For simple types, set the value directly
    //                    var convertedValue = Convert.ChangeType(prop.Value.ToString(), propertyInfo.PropertyType);
    //                    propertyInfo.SetValue(entity, convertedValue);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle exception if needed
    //    }
    //}
}