# Reminders

## About
The reminders app uses [Quartz.NET](http://www.quartz-scheduler.net) to scheduler reminders. The current reminders we have are.
* Trash 30
* Crossroads closing
* Salsarits closing
* Ctrl alt deli closing
* SSE Meeting

## Creating a Reminder
To create a reminder, we use cron syntax. Make a new method similar to the crossroads one below and call it in the constructor of `RemindersClient`.

```csharp
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
```
