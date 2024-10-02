using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;
using static Wheel.Application.Services.Concrete.GameService;

namespace Wheel.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IConsulGameService _consulGameService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoundRepository _roundRepository;
    private readonly IWheelPrizeRepository _wheelPrizeRepository;
    private readonly ISegmentRepository _segmentRepository;
    public GameService(
        ConfigurationHolder configurationHolder,
        IConfigurationRepository configurationRepository,
        IAuthService authService,
        IHubService hubService,
        IConsulGameService consulGameService,
        IUnitOfWork unitOfWork,
        IRoundRepository roundRepository,
        IWheelPrizeRepository wheelPrizeRepository,
        ISegmentRepository segmentRepository)
    {
        _configurationHolder = configurationHolder;
        _configurationRepository = configurationRepository;
        _authService = authService;
        _hubService = hubService;
        _consulGameService = consulGameService;
        _unitOfWork = unitOfWork;
        _roundRepository = roundRepository;
        _wheelPrizeRepository = wheelPrizeRepository;
        _segmentRepository = segmentRepository;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public GameResponseModel GetGame()
    {
        var segments = _configurationRepository.Query();

        return new GameResponseModel
        {
            Name = "Wheel",
            SegmentIds = segments == null ? default : segments.Select(x => x.Name).ToList(),
            ActivationTime = DateTime.UtcNow,
        };
    }

    public async Task UpdateMetadataAsync()
    {
        await _consulGameService.UpdateMetadataAsync(
            getDataFunc: GetGame,
            serviceId: "WheelApi",
            serviceName: "WheelApi",
            port: 8080,
            tags: ["Game", "Back"]
        );
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel model)
    {
        return await PlayAsync<JackpotPrize>(model);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<WheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel model)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(model.GameId, model.CurrencyId, model.Amount);

        //TODO: prioritetis minicheba segmentistvis
        var prize = GeneratorHolder.GetPrize<TPrize>(_authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(model.GameId, model.CurrencyId, model.Amount);
        }

        return new PlayResponseModel
        {
            PrizeResults = [prize],
            Multiplier = 0,
        };
    }

    public async Task<(ConfigurationModel, List<RoundModel>)> CreateConfigurationAndRoundsAsync(
    string configurationName, int configurationValue, string rule,
    List<RoundModel> rounds, List<PriceModel> prices = null, List<SegmentModel> segments = null)
    {
        // Create a new configuration without attaching segments yet
        var configuration = new Configuration(configurationName, configurationValue,
            prices?.Select(p => new Price(p.Value, p.Multiplier, p.CurrencyId, p.SegmentId)),
            null);

        await _configurationRepository.InsertAsync(configuration);

        // Ensure that you only add unique segments
        if (segments != null)
        {
            var validSegments = new List<Segment>();

            foreach (var segmentModel in segments)
            {
                // Check if segment already exists in the database
                var existingSegment = await _segmentRepository.Query().FirstOrDefaultAsync(x => x.Id == segmentModel.Id);

                if (existingSegment == null)
                {
                    // If segment doesn't exist, add it
                    var newSegment = new Segment(segmentModel.Id, configuration.Id);
                    validSegments.Add(newSegment);
                }
                else
                {
                    // If the segment already exists, associate it with the configuration
                    validSegments.Add(existingSegment);
                }
            }

            // Assign the valid segments to the configuration
            configuration.AssignSegments(validSegments);
        }

        // Save the configuration with associated valid segments
        await _unitOfWork.SaveAsync();

        // Process Rounds (as before)
        var createdRounds = new List<Round>();
        foreach (var roundModel in rounds)
        {

            // Create a new WheelPrize for each round
            var wheelPrize = new WheelPrize
            {
                WheelIndex = roundModel.WheelPrize.WheelIndex,  // Set WheelIndex
                Name = roundModel.WheelPrize.Name               // Set Name or other properties
            };

            await _wheelPrizeRepository.InsertAsync(wheelPrize);
            await _unitOfWork.SaveAsync();  // Save the wheelPrize to ensure it gets an ID

            // Create the Round and associate it with the Configuration and WheelPrize
            var round = new Round
            {
                Name = roundModel.Name,
                ConfigurationId = configuration.Id,
                WheelPrizeId = wheelPrize.Id,  // Associate the new WheelPrize by its ID
                WheelPrize = wheelPrize        // Set the created WheelPrize object
            };

            createdRounds.Add(round);
            await _roundRepository.InsertAsync(round);
        }

        await _unitOfWork.SaveAsync();

        // Prepare response models
        var configurationModel = new ConfigurationModel
        {
            Name = configuration.Name,
            Value = configuration.Value,
            Rule = configuration.Rule,
            IsActive = configuration.IsActive,
            Prices = configuration.Prices.Select(p => new PriceModel
            {
                Value = p.Value,
                Multiplier = p.Multiplier,
                CurrencyId = p.CurrencyId,
                SegmentId = p.SegmentId
            }).ToList(),
            Segments = segments
        };

        var roundModels = createdRounds.Select(r => new RoundModel
        {
            Name = r.Name,
            ConfigurationId = r.ConfigurationId,
            PrizeId = r.WheelPrizeId,
            WheelPrize = new PrizeModel
            {
                Id = r.WheelPrize.Id,
                Name = r.WheelPrize.Name
            }
        }).ToList();

        return (configurationModel, roundModels);
    }

    public class RoundModel
    {
        public string Name { get; set; }
        public int ConfigurationId { get; set; }
        public int PrizeId { get; set; }
        public PrizeModel WheelPrize { get; set; }
    }

    public class WheelPrizeModel
    {
        public int? WheelIndex { get; set; }
        public int? RoundId { get; set; }
        public RoundModel Round { get; set; }
    }

    public class ConfigurationModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool IsActive { get; set; }
        public string Rule { get; set; }
        public List<PriceModel> Prices { get; set; }
        public List<SegmentModel> Segments { get; set; }
    }

    public class CurrencyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class PriceModel
    {
        public decimal Value { get; set; }
        public decimal Multiplier { get; set; }
        public string CurrencyId { get; set; }
        public int SegmentId { get; set; }
    }

    public class PrizeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WheelIndex { get; set; }
    }

    public class SegmentModel
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public int? ConfigurationId { get; set; }
    }
}