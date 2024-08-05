using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test;

var serviceProvider = new ServiceCollection()
           .AddDbContext<GameDbContext>(options =>
               options.UseNpgsql("User ID =postgres;Password=1234;Server=localhost;Port=5432;Database=TestGame3;Pooling=true"))
           .BuildServiceProvider();

using (var context = serviceProvider.GetRequiredService<GameDbContext>())
{
    context.Database.EnsureCreated();

}

var p = new WheelPrizeGroup { test1 = 12, test = 14 };

Generator.AddGroup(p);

var t = Generator.GetPrize<WheelPrizeGroup>();
var L = new List<WheelPrizeGroup>();

Console.WriteLine("Hello, World!");
