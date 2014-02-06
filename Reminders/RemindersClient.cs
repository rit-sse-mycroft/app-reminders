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
    /// <summary>
    /// The reminders application
    /// </summary>
    class RemindersClient : Client
    {
        private IScheduler scheduler;
        private string status;

        /// <summary>
        /// Constructor for the reminders client
        /// </summary>
        /// <param name="manifest">The path to the app manifest</param>
        public RemindersClient(string manifest) : base(manifest)
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            scheduler = schedFact.GetScheduler();
            status = "down";

            Trash30();
            Crossroads();
            Salsaritas();
            CtrlAltDeli();
            SSEMeeting();

            handler.On("APP_MANIFEST_OK", AppManifestOk);
            handler.On("APP_DEPENDENCY", AppDependency);
            
        }

        #region Reminders
        /// <summary>
        /// Reminder for trash 30
        /// </summary>
        private void Trash30()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("trash30Job")
                .UsingJobData("phrase", "It's trash 30. Please pick up any trash around you and throw it away.")
                .Build();
            job.JobDataMap.Add("client", this);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trash30")
                .WithCronSchedule("0 30 10-18 * * ?")
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// Reminder for crossroads
        /// </summary>
        private void Crossroads()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("crossroadsJob")
                .UsingJobData("phrase", "Crossroads is closing in 20 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            Quartz.Collection.HashSet<ITrigger> set = new Quartz.Collection.HashSet<ITrigger>();
            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsWeekday")
                .WithCronSchedule("0 40 21 ? * MON-THU")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsFriday")
                .WithCronSchedule("0 40 17 ? * FRI")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("crossroadsSunday")
                .WithCronSchedule("0 40 19 ? * SUN")
                .Build());

            scheduler.ScheduleJob(job, set, true);
        }

        /// <summary>
        /// Reminder for Salsaritas
        /// </summary>
        private void Salsaritas()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("salsaritasJob")
                .UsingJobData("phrase", "Salsaritas is closing in 20 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            Quartz.Collection.HashSet<ITrigger> set = new Quartz.Collection.HashSet<ITrigger>();
            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasWeekday")
                .WithCronSchedule("0 40 20 ? * MON-THU")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasFriday")
                .WithCronSchedule("0 40 19 ? * FRI")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("salsaritasSaturday")
                .WithCronSchedule("0 40 19 ? * SAT")
                .Build());

            scheduler.ScheduleJob(job, set, true);
        }

        /// <summary>
        /// Reminder for Ctrl Alt Deli
        /// </summary>
        private void CtrlAltDeli()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("ctrlAltDeliJob")
                .UsingJobData("phrase", "Control Alt Deli is closing in 20 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            Quartz.Collection.HashSet<ITrigger> set = new Quartz.Collection.HashSet<ITrigger>();
            set.Add(TriggerBuilder.Create()
                .WithIdentity("ctrlAltDeliWeekday")
                .WithCronSchedule("0 10 18 ? * MON-THU")
                .Build());

            set.Add(TriggerBuilder.Create()
                .WithIdentity("ctrlAltDeliFriday")
                .WithCronSchedule("0 40 14 ? * FRI")
                .Build());

            scheduler.ScheduleJob(job, set, true);
 
        }

        /// <summary>
        /// Reminder for the SSE Meeting
        /// </summary>
        private void SSEMeeting()
        {
            IJobDetail job = JobBuilder.Create<MycroftJob>()
                .WithIdentity("sseMeetingJob")
                .UsingJobData("phrase", "The SSE Meeting is in 10 minutes")
                .Build();
            job.JobDataMap.Add("client", this);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("sseMeeting")
                .WithCronSchedule("0 50 16 ? * FRI")
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
        #endregion

        #region Message Helpers
        /// <summary>
        /// Called when app manifest okay is received
        /// </summary>
        /// <param name="obj">The message received</param>
        protected void AppManifestOk(dynamic obj)
        {
            InstanceId = obj["instanceId"];
        }

        /// <summary>
        /// Called when APP_DEPENDENCY is received
        /// </summary>
        /// <param name="message">The message received</param>
        protected async void AppDependency(dynamic message)
        {
            if(message["tts"].ContainsKey("text2speech"))
            {
                if (message["tts"]["text2speech"] == "up" && status == "down")
                {
                    await Up();
                    status = "up";
                    scheduler.Start();
                }
                else if (message["tts"]["text2speech"] == "down" && status == "up")
                {
                    await Down();
                    status = "down";
                    scheduler.Standby();
                }
            }
        }
        #endregion
    }
}
