using Consul;
using GameLib.Application.Controllers;
using GameLib.Application.Services.Concrete;
using GameLib.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            new Price(1, 5, string.Empty),
            new Price(2, 10, string.Empty),
            new Price(3, 15, string.Empty),
        };

        var segment = new Segment("Segment 1");

        var config = new WheelConfiguration("Wheel Configuration", 10, prices: prices, segments: [segment]);

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
