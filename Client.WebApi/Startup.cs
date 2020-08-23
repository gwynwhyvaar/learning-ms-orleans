using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
namespace Client.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
         public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IClusterClient>(connectClientAsync);
        }
        private IClusterClient connectClientAsync(IServiceProvider serviceProvider)
        {
            IClusterClient clusterClient;
            clusterClient = new ClientBuilder().UseLocalhostClustering().Configure<ClusterOptions>(options =>
            {
                /* the values here should match those set in the Silo */
                options.ClusterId = "orleans-dev-silo-host-#1";
                options.ServiceId = "orleans-dev-silo-service-#1";
            }).ConfigureLogging(_ => _.AddConsole()).Build();
            clusterClient.Connect().Wait();
            //Console.WriteLine($"Client successfully connected to silo  {DateTime.Today} \n");
            return clusterClient;
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
