using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Wheel.Api.Controllers;

[Route("wheelapi/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private static readonly ConcurrentDictionary<Guid, decimal> UserBalances = new ConcurrentDictionary<Guid, decimal>();

    public HomeController()
    {
        // Initialize user balance for the example
        var userId = new Guid("d271d93f-f736-4b2d-924d-55fe4b8462d1"); // Example user ID
        UserBalances.TryAdd(userId, 20.00m); // Each user starts with 20 GEL
    }

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
