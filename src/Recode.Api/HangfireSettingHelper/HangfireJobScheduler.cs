using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recode.Service.Implementations.Services;

namespace Recode.Api.HangfireSettingHelper
{
    public class HangfireJobScheduler
    {
        public static void SchedulerRecurringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(EmailSendingJob));
            RecurringJob.AddOrUpdate<EmailSendingJob>(nameof(EmailSendingJob),
                job => job.Run(JobCancellationToken.Null), Cron.MinuteInterval(5), TimeZoneInfo.Local);
        }
    }
}
