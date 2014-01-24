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
            scheduler.Start();
            status = down;
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

        protected override void Response(APP_MANIFEST_OK type, dynamic obj)
        {
            Console.WriteLine("Recieved Message " + type);
            InstanceId = obj["instanceId"];
        }

        protected async override void Response(APP_DEPENDENCY type, dynamic message)
        {
            try
            {
                if (message["tts"]["text2speech"] == "up" && status == "down")
                    await SendData("APP_UP", "");
                else if (message["tts"]["text2speech"] == "down" && status == "up")
                    await SendData("APP_DOWN", "");
            }
            catch
            {

            }

        }
    }
}
