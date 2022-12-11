using CoreQuartzExample.Models;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreQuartzExample.Services
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<MyJobs> _myJobs;

        public QuartzHostedService(ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<MyJobs> myJobs)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _myJobs = myJobs;
        }

        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Common.Logs($"StartAsync at " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"), "StartAsync at" + DateTime.Now.ToString("hhmmss"));

            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var myJob in _myJobs)
            {
                var job = CreateJob(myJob);
                var trigger = CreateTrigger(myJob);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Common.Logs($"StopAsync at " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"), "StopAsync at" + DateTime.Now.ToString("hhmmss"));

            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(MyJobs myJob)
        {
            var type = myJob.Type;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }
        private static ITrigger CreateTrigger(MyJobs myJob)
        {
            var type = myJob.Type;
            return TriggerBuilder.Create().WithIdentity($"{ myJob.Type.FullName}.trigger").WithCronSchedule(myJob.Expression).WithDescription(myJob.Expression).Build();
        }
    }
}
