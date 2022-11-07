using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dtx;

public class Program
{
    static void Main(string[] args)
    {
        using (IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(builder => builder
                        .ClearProviders()
                        .AddTextFileLogger("log.txt"))
                    .Build())
        {
            if (IntegrationTests.RunTests(host.Services))
            {
                Console.WriteLine("All integration tests passed OK.");
            }
            else
            {
                Console.WriteLine("ERROR(S) in integration tests.");
            }
        }
    }
}