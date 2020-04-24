using Amazon.Textract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RJM.BackgroundTasks.Services;
using System;

namespace RJM.BackgroundTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // RJM.API
                    services.AddHttpClient("RJM.API", options =>
                    {
                        options.BaseAddress = new Uri(hostContext.Configuration.GetValue<string>("API"));
                    });
                    services.AddSingleton<APIService>();

                    // Amazon AWS
                    services.AddDefaultAWSOptions(hostContext.Configuration.GetAWSOptions());
                    //// Textract
                    services.AddAWSService<IAmazonTextract>();
                    services.AddSingleton<AmazonTextractTextDetectionService>();

                    // Document parser
                    services.AddHostedService<DocumentParser>();
                });
    }
}
