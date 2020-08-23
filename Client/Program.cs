using GrainInterfaces.SmsSender;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;
namespace Client
{
    class Program
    {
        private static async Task<IClusterClient> connectClientAsync()
        {
            IClusterClient clusterClient;
            clusterClient = new ClientBuilder().UseLocalhostClustering().Configure<ClusterOptions>(options =>
              {
                  /* the values here should match those set in the Silo */
                  options.ClusterId = "orleans-dev-silo-host-#1";
                  options.ServiceId = "orleans-dev-silo-service-#1";
              }).ConfigureLogging(logging => logging.AddConsole()).Build();
            await clusterClient.Connect();
            Console.WriteLine($"Client successfully connected to silo  {DateTime.Today} \n");
            return clusterClient;
        }
        private static async Task processAsync(IClusterClient clusterClient)
        {
            // initialize sms grain
            // todo: what's the purpose of the 0 here?
            var sendSmsGrain = clusterClient.GetGrain<ISendSms>(0);
            var response = await sendSmsGrain.SendMessage("+2348125932888", "Hi there!");
            Console.WriteLine($"\n\n{response}\n\n");
        }
        private static async Task<int> runMainAsync()
        {
            try
            {
                using(var client = await connectClientAsync())
                {
                    await processAsync(client);
                    Console.ReadKey();
                }
                return 0;
            }
            catch(Exception exception)
            {
                Console.WriteLine($"\nException while trying to run client: {exception.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }
        static int Main(string[] args)
        {
            return runMainAsync().Result;
        }
    }
}
