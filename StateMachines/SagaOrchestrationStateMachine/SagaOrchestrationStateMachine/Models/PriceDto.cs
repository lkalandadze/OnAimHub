﻿namespace SagaOrchestrationStateMachine.Models;

public class PriceDto
{
    public string Id { get; set; }
    public int Value { get; set; }
    public int Multiplier { get; set; }
    public string CoinId { get; set; }
}