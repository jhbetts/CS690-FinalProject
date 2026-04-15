using Quartz;
using Quartz.Impl;

namespace Game;

class Program
{
    static async Task Main(string[] args)
    {
        var dataManager = new DataManager();

        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        var job = JobBuilder.Create<AlertEmailJob>()
            .WithIdentity("alert-email-job")
            .StoreDurably()
            .UsingJobData(new JobDataMap
            {
                ["DataManager"] = dataManager
            })
            .Build();
        await scheduler.AddJob(job, replace: true);

        for (int i = 0; i < dataManager.Alerts.Count; i++)
        {
            var alert = dataManager.Alerts[i];
            await ScheduleAlertTrigger(scheduler, job, alert, i);
        }

        var theUI = new ConsoleUI(dataManager, scheduler, job);
        await theUI.Show();
        await scheduler.Shutdown(waitForJobsToComplete:false);
    }

    public static Task ScheduleAlertTrigger(IScheduler scheduler, IJobDetail job, Alert alert, int alertIndex)
    {
        var cron = $"0 {alert.SendTime.Minute} {alert.SendTime.Hour} * * ?";
        var trigger = TriggerBuilder.Create()
            .WithIdentity($"alert-trigger-{alertIndex}-{alert.SendTime:yyyyMMddHHmmss}")
            .ForJob(job)
            .WithCronSchedule(cron)
            .Build();

        return scheduler.ScheduleJob(trigger);
    }

}

