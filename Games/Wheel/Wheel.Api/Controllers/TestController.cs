﻿using GameLib.Application.Controllers;
using GameLib.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;
using Wheel.Domain.Entities;

namespace Wheel.Api.Controllers;

public class TestController : BaseApiController
{
    private static readonly ConcurrentDictionary<Guid, decimal> UserBalances = new ConcurrentDictionary<Guid, decimal>();

    public TestController()
    {
        // Initialize user balance for the example
        var userId = new Guid("d271d93f-f736-4b2d-924d-55fe4b8462d1"); // Example user ID
        UserBalances.TryAdd(userId, 20.00m); // Each user starts with 20 GEL
    }

    [HttpPost("AddConfigTest")]
    public ActionResult AddConfigTest()
    {
        var prices = new List<Price>
        {
            new Price(1, 5, "OnAimCoin"),
            new Price(2, 10, string.Empty),
            new Price(3, 15, "OnAimCoin"),
        };

        var segments = new List<Segment>
        {
            new Segment("Segment 1"),
            new Segment("Segment 2"),
        };

        var prizes1 = new List<WheelPrize>
        {
            new WheelPrize("R1 P1", 1) { Value = 1, Probability = 10 },
            new WheelPrize("R1 P2", 2) { Value = 2, Probability = 20 },
            new WheelPrize("R1 P3", 3) { Value = 3, Probability = 30 },
        };

        var prizes2 = new List<WheelPrize>
        {
            new WheelPrize("R2 P1", 1) { Value = 1, Probability = 15 },
            new WheelPrize("R2 P2", 2) { Value = 2, Probability = 25 },
            new WheelPrize("R2 P3", 3) { Value = 3, Probability = 35 },
        };

        var rounds = new List<Round>
        {
            new Round("R1", prizes1),
            new Round("R2", prizes2),
        };

        var config = new WheelConfiguration("Wheel Configuration", 10, prices: prices, segments: segments, rounds: rounds);

        //var rootContainers = CheckmateValidations.Checkmate.GetRootCheckContainers(config.GetType());

        var rootCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(config);
        var treeCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(config, "", true);

        //var rootCheckers = CheckmateValidations.Checkmate.GetChecks(config).ToList();
        //var treeCheckers = CheckmateValidations.Checkmate.GetChecks(config, true).ToList();

        var rootFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(config).ToList();
        var treeFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(config, true).ToList();

        var rootStatus = CheckmateValidations.Checkmate.IsValid(config);
        var treeStatus = CheckmateValidations.Checkmate.IsValid(config, true);

        string json1 = JsonSerializer.Serialize(config);
        string json2 = JsonSerializer.Serialize(treeFailedCheckers);
        string json3 = JsonSerializer.Serialize(treeCheckContainer);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet(nameof(HealthCheck))]
    public IActionResult HealthCheck()
    {
        return Ok();
    }

    [HttpPost("spin")]
    public IActionResult SpinWheel([FromBody] Guid userId)
    {
        if (!UserBalances.ContainsKey(userId))
        {
            return BadRequest("User not found.");
        }

        if (UserBalances[userId] < 10)
        {
            return BadRequest("Insufficient balance.");
        }

        // Deduct the spin cost
        UserBalances[userId] -= 10;

        // Simulate a spin result
        var random = new Random();
        var isWin = random.Next(0, 2) == 1; // 50% chance to win
        var winnings = isWin ? random.Next(10, 101) : 0; // Random win amount between 10 and 100 GEL if win

        if (isWin)
        {
            // Add winnings to the balance
            UserBalances[userId] += winnings;
        }

        return Ok(new
        {
            Message = isWin ? "Congratulations, you won!" : "Sorry, you lost.",
            Winnings = winnings,
            Balance = UserBalances[userId]
        });
    }

    [HttpGet("balance/{userId}")]
    public IActionResult GetBalance(Guid userId)
    {
        if (!UserBalances.ContainsKey(userId))
        {
            return BadRequest("User not found.");
        }

        return Ok(new { Balance = UserBalances[userId] });
    }
}
