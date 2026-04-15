using Quartz;

namespace Game;

public class AlertEmailJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var dataManager = (DataManager)context.JobDetail.JobDataMap["DataManager"];
        dataManager.SendAlertEmails();
        return Task.CompletedTask;
    }
}