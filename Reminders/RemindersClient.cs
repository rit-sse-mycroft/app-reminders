using Mycroft.App;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminders
{
    class RemindersClient : Client
    {
        private IScheduler scheduler;

        public RemindersClient() : base()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            scheduler = schedFact.GetScheduler();
            scheduler.Start();

            Trash30();
        }

        public void Trash30()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("trash30", "reminder")
                .UsingJobData("phrase", "It's trash 30. Please pick up any trash around you and throw it away.")
                .Build();
            job.JobDataMap.Add("client", this);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trash30Trigger", "reminder")
                .WithCronSchedule("30 10-18 * * *")
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}
