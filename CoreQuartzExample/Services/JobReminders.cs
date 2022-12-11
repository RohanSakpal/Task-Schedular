using CoreQuartzExample.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace CoreQuartzExample.Services
{
    public class JobReminders : IJob
    {
        public JobReminders()
        {

        }

        public Task Execute(IJobExecutionContext context)
        {
            Common.Logs($"JobReminders at " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"), "JobReminders" + DateTime.Now.ToString("hhmmss"));
            return Task.CompletedTask;

        }
    }
}
