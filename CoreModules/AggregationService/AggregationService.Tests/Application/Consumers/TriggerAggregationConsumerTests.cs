using AggregationService.Application.Consumers.Trigger;
using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;
using MassTransit;
using MongoDB.Driver;
using Moq;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Tests.Application.Consumers;

public class TriggerAggregationConsumerTests
{
    private readonly Mock<IAggregationConfigurationService> _mockService;
    private readonly Mock<IAggregationConfigurationRepository> _mockRepository;
    private readonly Mock<IConfigurationStore> _mockStore;
    private readonly Mock<ConsumeContext<AggregationTriggerEvent>> _mockContext;
    private readonly TriggerAggregationConsumer _consumer;

    public TriggerAggregationConsumerTests()
    {
        _mockService = new Mock<IAggregationConfigurationService>();
        _mockRepository = new Mock<IAggregationConfigurationRepository>();
        _mockStore = new Mock<IConfigurationStore>();
        _mockContext = new Mock<ConsumeContext<AggregationTriggerEvent>>();

        _consumer = new TriggerAggregationConsumer(
            _mockService.Object,
            _mockRepository.Object,
            _mockStore.Object
        );
    }

    [Fact]
    public async Task Consume_ShouldProcessAggregation_WhenConfigurationsExist()
    {
        // Arrange
        var triggerEvent = new AggregationTriggerEvent(
            data: "{ \"customerId\":\"11\", \"Coin\":\"1_OnAimCoin\", \"TransactionType\":\"Bet\", \"Value\":\"100\", \"promotionId\":\"1\" }",
            producer: "Hub"
        );

        _mockContext.Setup(c => c.Message).Returns(triggerEvent);
        _mockContext.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        var filters = new List<Filter>
        {
            new Filter("Coin", Operator.Equals, "1_OnAimCoin"),
            new Filter("TransactionType", Operator.Equals, "Bet"),
            new Filter("promotionId", Operator.Equals, "1")
        };

        var configurations = new List<AggregationConfiguration>
        {
            new AggregationConfiguration(
                name: "UnitTest",
                description: "Test Configuration",
                eventProducer: "Hub",
                aggregationSubscriber: "Leaderboard",
                filters: filters,
                aggregationType: AggregationType.Sum,
                evaluationType: EvaluationType.SingleRule,
                pointEvaluationRules: new List<PointEvaluationRule>
                {
                    new PointEvaluationRule(1, 10)
                },
                selectionField: "Value",
                expiration: DateTime.UtcNow.AddDays(365),
                promotionId: "1",
                key: "1"
            )
        };

        var mockCollection = new Mock<IMongoCollection<AggregationConfiguration>>();
        var mockCursor = new Mock<IAsyncCursor<AggregationConfiguration>>();

        mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true) // First call to MoveNext returns true
            .Returns(false); // End of cursor
        mockCursor.Setup(x => x.Current).Returns(configurations);

        _mockRepository.Setup(r => r.GetCollection()).Returns(mockCollection.Object);
        mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<AggregationConfiguration>>(),
                It.IsAny<FindOptions<AggregationConfiguration>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        await _consumer.Consume(_mockContext.Object);

        // Assert
        _mockStore.Verify(s => s.ReloadConfigurationsAsync(), Times.Once);
        _mockService.Verify(s => s.TriggerRequestAsync(triggerEvent, configurations.First()), Times.Once);
    }

    [Fact]
    public async Task Consume_ShouldThrowException_WhenNoConfigurationsMatch()
    {
        // Arrange
        var triggerEvent = new AggregationTriggerEvent(
            data: "{\"customerId\":\"11\",\"producer\":\"Hub\"}",
            producer: "Hub"
        );

        _mockContext.Setup(c => c.Message).Returns(triggerEvent);

        var mockCollection = new Mock<IMongoCollection<AggregationConfiguration>>();
        var mockCursor = new Mock<IAsyncCursor<AggregationConfiguration>>();

        // Simulate no configurations returned
        mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(false); // No matching data

        _mockRepository.Setup(r => r.GetCollection()).Returns(mockCollection.Object);
        mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<AggregationConfiguration>>(),
                It.IsAny<FindOptions<AggregationConfiguration>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _consumer.Consume(_mockContext.Object));
    }

    //[Fact]
    //public async Task Consume_ShouldProcessExternalEvent_WhenIsExternalIsTrue()
    //{
    //    // Arrange
    //    var triggerEvent = new AggregationTriggerEvent(
    //        data: "{\"customerId\":\"12345\",\"producer\":\"external\"}",
    //        producer: "external"
    //    );

    //    _mockContext.Setup(c => c.Message).Returns(triggerEvent);

    //    var configurations = new List<AggregationConfiguration>
    //{
    //    new AggregationConfiguration(
    //        name: "ExternalConfig",
    //        description: "External Test Configuration",
    //        eventProducer: "external",
    //        aggregationSubscriber: "Leaderboard",
    //        filters: new List<Filter>(),
    //        aggregationType: AggregationType.Count,
    //        evaluationType: EvaluationType.SingleRule,
    //        pointEvaluationRules: new List<PointEvaluationRule>(),
    //        selectionField: "None",
    //        expiration: DateTime.UtcNow.AddDays(7),
    //        promotionId: "Promo2",
    //        key: "Key2"
    //    )
    //};

    //    // Mock the IAsyncCursor for configurations
    //    var mockCursor = new Mock<IAsyncCursor<AggregationConfiguration>>();
    //    mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
    //        .Returns(true)  // First call to MoveNext returns true
    //        .Returns(false); // Second call returns false (end of cursor)
    //    mockCursor.Setup(x => x.Current).Returns(configurations);

    //    // Setup the repository to return the mocked cursor
    //    _mockRepository
    //        .Setup(r => r.GetCollection().FindAsync(It.IsAny<FilterDefinition<AggregationConfiguration>>(), null, CancellationToken.None))
    //        .ReturnsAsync(mockCursor.Object);

    //    // Act
    //    await _consumer.Consume(_mockContext.Object);

    //    // Assert
    //    _mockService.Verify(s => s.TriggerRequestAsync(triggerEvent, configurations.First()), Times.Once);
    //}
}
