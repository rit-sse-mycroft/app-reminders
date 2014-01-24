using Mycroft.App;
using Mycroft.App.Message;
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
        private string status;

        public RemindersClient() : base()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            scheduler = schedFact.GetScheduler();
            status = "down";
            Trash30();
            Crossroads();
            Salsaritas();
        }

        private void Trash30()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("trash30Job")
                .UsingJobData("phrase", "It's trash 30. Please pick up any trash around you and throw it away.")
                .Build();
            job.JobDataMap.Add("client", this);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trash30Trigger")
                .WithCronSchedule("0 30 10-18 * * *")
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }

        private void Crossroads()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("crossroadsJob")
                .UsingJobData("phrase", "Crossroads is closing in 20 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            Quartz.Collection.HashSet<ITrigger> set = new Quartz.Collection.HashSet<ITrigger>();
            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsWeekdayTrigger")
                .WithCronSchedule("0 40 21 * * MON-THU")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsFriday")
                .WithCronSchedule("0 40 17 * * FRI")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsSunday")
                .WithCronSchedule("0 40 19 * * SUN")
                .Build());

            scheduler.ScheduleJob(job, set, true);
        }

        private void Salsaritas()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("salsaritasJob")
                .UsingJobData("phrase", "Salsaritas is closing in 20 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            Quartz.Collection.HashSet<ITrigger> set = new Quartz.Collection.HashSet<ITrigger>();
            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasWeekdayTrigger")
                .WithCronSchedule("0 40 20 * * MON-THU")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasFriday")
                .WithCronSchedule("0 40 19 * * FRI")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasSaturday")
                .WithCronSchedule("0 40 19 * * SAT")
                .Build());

            scheduler.ScheduleJob(job, set, true);
        }

        protected override void Response(APP_MANIFEST_OK type, dynamic obj)
        {
            Console.WriteLine("Recieved Message " + type);
            InstanceId = obj["instanceId"];
        }

        protected async override void Response(APP_DEPENDENCY type, dynamic message)
        {
            if(message["tts"].ContainsKey("text2speech"))
            {
                if (message["tts"]["text2speech"] == "up" && status == "down")
                {
                    await SendData("APP_UP", "");
                    status = "up";
                    scheduler.Start();
                }
                else if (message["tts"]["text2speech"] == "down" && status == "up")
                {
                    await SendData("APP_DOWN", "");
                    status = "down";
                    scheduler.Standby();
                }
            }
        }
    }
}
