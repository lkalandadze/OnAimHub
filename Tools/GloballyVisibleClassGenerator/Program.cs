using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        try
        {

            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);

            //args = ["C:\\Users\\LashaKalandadze\\source\\repos\\OnAimHub\\Hub\\Hub.Domain\\bin\\Debug\\net8.0\\Hub.Domain.dll",
            //    "C:\\Users\\LashaKalandadze\\source\\repos\\OnAimHub\\Admin\\OnAim.Admin.Infrasturcture\\HubEntities",
            //JsonConvert.SerializeObject(new GloballyVisibleClassGeneratorParams(){ CopyBaseClasses = true, Namespace = "NMSPC"})];

            if (args.Length > 2)
            {
                Console.WriteLine(args[2]);
            }

            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            Generator.Generate(args, logger);
            Console.WriteLine("Code Generated!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Code Generation Failed!");
            Console.WriteLine(ex.ToString());
        }

        Environment.Exit(0);
    }
}