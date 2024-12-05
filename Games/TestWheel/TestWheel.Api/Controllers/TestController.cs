//using GameLib.Application.Controllers;
//using GameLib.Application.Generators;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Collections.Concurrent;

//namespace TestWheel.Api.Controllers;

//public class TestController : BaseApiController
//{
//    private static readonly ConcurrentDictionary<Guid, decimal> UserBalances = new ConcurrentDictionary<Guid, decimal>();
//    private readonly EntityGenerator _entityGenerator;

//    public TestController(EntityGenerator entityGenerator)
//    {
//        _entityGenerator = entityGenerator;

//        // Initialize user balance for the example
//        var userId = new Guid("d271d93f-f736-4b2d-924d-55fe4b8462d1"); // Example user ID
//        UserBalances.TryAdd(userId, 20.00m); // Each user starts with 20 GEL
//    }

//    [HttpGet("TestJson")]
//    public ActionResult<EntityMetadata> GetTestJson()
//    {
//        #region Configurations

//        //var prices = new List<Price>
//        //{
//        //    new Price(1, 5, "OnAimCoin"),
//        //    new Price(2, 10, string.Empty),
//        //    new Price(3, 15, "OnAimCoin"),
//        //};

//        //var prizes1 = new List<WheelPrize>
//        //{
//        //    new WheelPrize("R1 P1", 1) { Value = 1, Probability = 10 },
//        //    new WheelPrize("R1 P2", 2) { Value = 2, Probability = 20 },
//        //    new WheelPrize("R1 P3", 3) { Value = 3, Probability = 30 },
//        //};

//        //var prizes2 = new List<WheelPrize>
//        //{
//        //    new WheelPrize("R2 P1", 1) { Value = 1, Probability = 15 },
//        //    new WheelPrize("R2 P2", 2) { Value = 2, Probability = 25 },
//        //    new WheelPrize("R2 P3", 3) { Value = 3, Probability = 35 },
//        //};

//        //var rounds = new List<Round>
//        //{
//        //    new Round("R1", prizes1),
//        //    new Round("R2", prizes2),
//        //};

//        //var config = new WheelConfiguration("Wheel Configuration", 10, prices: prices, segments: segments, rounds: rounds);

//        //var rootCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(config);
//        //var treeCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(config, "", true);

//        //var rootFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(config).ToList();
//        //var treeFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(config, true).ToList();

//        //var rootStatus = CheckmateValidations.Checkmate.IsValid(config);
//        //var treeStatus = CheckmateValidations.Checkmate.IsValid(config, true);

//        //string json1 = JsonSerializer.Serialize(config);
//        //string json2 = JsonSerializer.Serialize(treeFailedCheckers);
//        //string json3 = JsonSerializer.Serialize(treeCheckContainer);

//        #endregion

//        #region TestForJson

//        //var e = new E
//        //{
//        //    NumberE = -1
//        //};

//        //var d = new D
//        //{
//        //    NumberD = - 1,
//        //    E = [e],
//        //};

//        //var c1 = new C1
//        //{
//        //    NumberC1 = -1,
//        //    D = [d],
//        //};

//        //var b1 = new B1
//        //{
//        //    NumberB1 = -1,
//        //    C1 = [c1],
//        //};

//        //var a = new A
//        //{
//        //    NumberA = -1,
//        //    B1 = [b1],
//        //};

//        //var aRootCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(a);
//        //var aTreeCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(a, "", true);

//        //string jsonA = JsonSerializer.Serialize(aRootCheckContainer);
//        //string jsonA1 = JsonSerializer.Serialize(aTreeCheckContainer);

//        //var aMetaData = _entityGenerator.GenerateEntityMetadata(typeof(A));

//        //string jsonA2 = JsonSerializer.Serialize(aMetaData);

//        #endregion

//        return Ok(/*aMetaData*/);
//    }

//    [HttpPost("CreateObjectFromJson")]
//    public ActionResult CreateObjectTest(TestObjectModel model)
//    {
//        //var data = JsonConvert.DeserializeObject<A>(model.ObjectJson);

//        var treeCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(data, "", true);
//        var treeFailedChecks = CheckmateValidations.Checkmate.GetFailedChecks(data, true);

//        var json1 = JsonConvert.SerializeObject(treeCheckContainer);
//        var json2 = JsonConvert.SerializeObject(treeFailedChecks);

//        return StatusCode(201);
//    }

//    [AllowAnonymous]
//    [HttpGet(nameof(HealthCheck))]
//    public IActionResult HealthCheck()
//    {
//        return Ok();
//    }

//    [HttpPost("spin")]
//    public IActionResult SpinWheel([FromBody] Guid userId)
//    {
//        if (!UserBalances.ContainsKey(userId))
//        {
//            return BadRequest("User not found.");
//        }

//        if (UserBalances[userId] < 10)
//        {
//            return BadRequest("Insufficient balance.");
//        }

//        // Deduct the spin cost
//        UserBalances[userId] -= 10;

//        // Simulate a spin result
//        var random = new Random();
//        var isWin = random.Next(0, 2) == 1; // 50% chance to win
//        var winnings = isWin ? random.Next(10, 101) : 0; // Random win amount between 10 and 100 GEL if win

//        if (isWin)
//        {
//            // Add winnings to the balance
//            UserBalances[userId] += winnings;
//        }

//        return Ok(new
//        {
//            Message = isWin ? "Congratulations, you won!" : "Sorry, you lost.",
//            Winnings = winnings,
//            Balance = UserBalances[userId]
//        });
//    }

//    [HttpGet("balance/{userId}")]
//    public IActionResult GetBalance(Guid userId)
//    {
//        if (!UserBalances.ContainsKey(userId))
//        {
//            return BadRequest("User not found.");
//        }

//        return Ok(new { Balance = UserBalances[userId] });
//    }
//}

//public class TestObjectModel
//{
//    public string ObjectJson { get; set; }
//}