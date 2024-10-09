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

        var data = JsonConvert.DeserializeObject<WheelConfiguration>(request.ConfigurationJson);
        await _wheelConfigurationRepository.InsertAsync(data);
        try
        {
            await _wheelConfigurationRepository.SaveAsync();
        }
        catch (Exception ex) { }

    }
}
//        string hardcodedJson = @"
//        {
//    'Name': 'Hardcoded Wheel Configuration',
//    'Value': 1000,
//    'IsActive': true,
//    'Rounds': [
//        {
//            'Name': 'Hardcoded Round 1',
//            'Sequence': [1, 2, 3],
//            'Prizes': [
//                {
//                    'Name': ""Test"",
//                    'Value': 100,
//                    'Probability': 50,
//                    'PrizeTypeId': 1,
//                    'WheelIndex': 0
//                },
//                {
//                    'Name': ""Test"",
//                    'Value': 200,
//                    'Probability': 30,
//                    'PrizeTypeId': 2,
//                    'WheelIndex': 1
//                }
//            ]
//        },
//        {
//            'Name': 'Hardcoded Round 2',
//            'Sequence': [1, 2, 3],
//            'Prizes': [
//                {
//                    'Name': ""Test"",
//                    'Value': 150,
//                    'Probability': 40,
//                    'PrizeTypeId': 1,
//                    'WheelIndex': 2
//                },
//                {
//                    'Name': ""Test"",
//                    'Value': 250,
//                    'Probability': 20,
//                    'PrizeTypeId': 3,
//                    'WheelIndex': 3
//                }
//            ]
//        }
//    ],
//    'Prices': [
//        {
//            'Id': 'Price5',
//            'Value': 200.50,
//            'Multiplier': 1.5,
//            'CurrencyId': 'OnAimCoin'
//        },
//        {
//            'Id': 'Price6',
//            'Value': 350.75,
//            'Multiplier': 2.0,
//            'CurrencyId': 'OnAimCoin'
//        }
//    ],
//    'Segments': [
//        {
//            'Id': 'Segment5',
//            'IsDeleted': true
//        },
//        {
//            'Id': 'Segment6',
//            'IsDeleted': false
//        }
//    ]
//}
//";