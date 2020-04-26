using Amazon.S3;
using Amazon.Textract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RJM.BackgroundTasks.Services;
using RJM.BackgroundTasks.Services.Files;
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
                    //// S3
                    services.AddAWSService<IAmazonS3>();
                    services.AddSingleton<AWSS3Service>();
                    //// Textract
                    services.AddAWSService<IAmazonTextract>();
                    services.AddSingleton<AWSTextractService>();

                    // Background tasks
                    //// Document parser
                    services.AddHostedService<DocumentParser>();
                });
    }
}
