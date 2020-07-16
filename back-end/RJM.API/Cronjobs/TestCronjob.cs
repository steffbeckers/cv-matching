using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using RJM.API.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RJM.API.Cronjobs
{
    public class TestCronjob : BackgroundService
    {
        private CrontabSchedule schedule;
        private DateTime nextRun;
        public IServiceProvider services { get; }

        private string Schedule => "*/10 * * * * *"; //Runs every 10 seconds

        public TestCronjob(IServiceProvider services)
        {
            this.services = services;
            this.schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            this.nextRun = schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = schedule.GetNextOccurrence(now);
                if (now > nextRun)
                {
                    Process();
                    nextRun = schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private async Task Process()
        {
            //using (var scope = this.services.CreateScope())
            //{
            //    SkillBLL skillBLL = scope.ServiceProvider.GetRequiredService<SkillBLL>();

            //    await skillBLL.CreateSkillAsync(new Models.Skill()
            //    {
            //        DisplayName = "Jonas"
            //    });
            //}

            Console.WriteLine("TestCronjob ran on " + DateTime.Now.ToString("F"));
        }
    }
}
