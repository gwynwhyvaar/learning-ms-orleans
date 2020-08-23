using Grains.SmsSender;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
namespace Silo.CmdConsole
{
    class Program
    {
        private static async Task<ISiloHost> startSiloAsync()
        {
            var builder = new SiloHostBuilder().UseLocalhostClustering().Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "orleans-dev-silo-host-#1";
                options.ServiceId = "orleans-dev-silo-service-#1";
            }).ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(SendSms).Assembly).WithReferences()).UseDashboard(_ => { _.Port = 2020; _.Password = "test123"; _.Username = "admin"; _.Host = "*"; _.HostSelf = true; }).ConfigureLogging(logging => logging.AddConsole());
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
        private static async Task<int> runMainAsync()
        {
            try
            {
                var host = await startSiloAsync();
                Console.WriteLine($"\n\nSilo Started {DateTime.Today}\nPress ENTER or ANY KEY to terminate...\n\n");
                Console.ReadLine();
                await host.StopAsync();
                return 0;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return -1;
            }
        }
        static int Main(string[] args)
        {
            return runMainAsync().Result;
        }
    }
}
