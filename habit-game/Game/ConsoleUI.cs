namespace Game;

using System.Net.Mail;
using Quartz;
using Spectre.Console;

public class ConsoleUI
{
    DataManager dataManager;
    IScheduler scheduler;
    IJobDetail alertJob;


    public ConsoleUI(DataManager dataManager, IScheduler scheduler, IJobDetail alertJob)
    {
        this.dataManager = dataManager;
        this.scheduler = scheduler;
        this.alertJob = alertJob;
        this.dataManager.SetTodaysScore(dataManager.TodaysScore, dataManager.Habits);
    }
    public async Task Show()
    {
        AnsiConsole.Clear();

        bool running = true;
        while (running)
        {
            var score = dataManager.TodaysScore;
            var panel = new Panel(score.ToString());
            AnsiConsole.Write(panel);
            string mode = "";
            if (mode == "") ;
            {
                mode = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select Mode")
                        .AddChoices("Add Habit", 
                        "Remove Habit", 
                        "Add Occurrence", 
                        "View Score Details", 
                        "View Habit Details", 
                        "Reset Score", 
                        "Create New Alert",
                        "Setup Alert Service",
                        "Quit")
                );
            }
            
            if (mode == "Add Habit")
            {
                AddHabitUI();
            }            
            if (mode == "Remove Habit")
            {
                RemoveHabitUI();
            }
            if (mode == "Add Occurrence")
            {
                AddPoint();
            }
            if (mode == "View Habit Details")
            {
                ViewHabitDetails();
            }
            if (mode == "Reset Score")
            {
                dataManager.ResetScores(dataManager.Habits);
            }
            if (mode == "View Score Details")
            {
                CheckScore(score);
            }
            if (mode == "Create New Alert")
            {
                await CreateAlert();
            }
            if (mode == "Setup Alert Service")
            {
                SetupAlerts();
            }

            if (mode == "Quit")
            {
                var scores = dataManager.Scores;
                int index = scores.FindIndex(x => x.Date == DateTime.Today);
                if (index != -1)
                {
                    scores[index] = score;
                } else
                {
                    scores.Add(score);  
                }

                dataManager.saveScore(scores);
                running = false;
            }
            AnsiConsole.Clear();
        }
    }
    private void SetupAlerts()
    {
        string mailServer = AnsiConsole.Ask<string>("Input SMTP Server URL: ");
        string mailUsername = AnsiConsole.Ask<string>("Input SMTP Server Username: ");
        string mailPassword = AnsiConsole.Ask<string>("Input SMTP Server Password: ");
        int mailPort = AnsiConsole.Ask<int>("Input SMTP Server Port: ");
        var SSLString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Is SSL/TSL required?")
            .AddChoices("Yes", "No")
        );
        var SSLRequired = SSLString == "Yes";
        dataManager.AddMailservice(mailServer, mailUsername, mailPassword, mailPort, SSLRequired);
    }
    private void SendAlert()
    {
        dataManager.SendAlertEmails();
    }
    private async Task CreateAlert()
    {
        string toEmail = AnsiConsole.Ask<string>("To: ");
        DateTime alertTime;
        string prompt = "Enter time to send alert email in 24 hour format (e.g. 22:00 = 10:00PM): ";
        Console.WriteLine(prompt);
        string time = Console.ReadLine();
        while(!DateTime.TryParse(time, out alertTime))
        {
            Console.WriteLine("Time invalid, try again.");
            Console.WriteLine(prompt);
            time = Console.ReadLine();
        }


        var alert = new Alert(To: toEmail, SendTime: alertTime);
        dataManager.addAlert(alert);
        await Program.ScheduleAlertTrigger(scheduler, alertJob, alert, dataManager.Alerts.Count - 1);
    }
    private void CheckScore(Score score)
    {
        var table = new Table();

        // Columns
        table.AddColumn("Name");
        table.AddColumn("Score");
        table.AddColumn("Good/Bad");
        for (int i = 0; i < dataManager.Habits.Count(); i++)
        {
            table.AddRow(dataManager.Habits[i].Name, dataManager.Habits[i].DisplayProgress(), dataManager.Habits[i].GoodBad());
        }
        AnsiConsole.Write(table);

        AnsiConsole.WriteLine("Press any key to continue.");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    public void AddHabitUI()
    {
        var name = AnsiConsole.Ask<string>("Name: ");
        var goal = AnsiConsole.Ask<int>("Daily goal/limit: ");
        var goodBadString = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Is this a good or bad habit?")
            .AddChoices("Good", "Bad")
        );
        var goodBad = goodBadString == "Good";
        Habit newHabit = new Habit(Name:name, Goal:0, Score:0, IsGoodHabit:goodBad);
        newHabit.SetGoal(goal);
        dataManager.addHabit(newHabit);
    }

    public void RemoveHabitUI()
    {
        if (dataManager.Habits.Count > 0)
            {
                var habitToRemove = AnsiConsole.Prompt(
                    new SelectionPrompt<Habit>()
                    .Title("Select habit to remove")
                    .AddChoices(dataManager.Habits)
                );

                dataManager.removeHabit(habitToRemove);
            } else
            {
                AnsiConsole.Write("No habits to remove");
                AnsiConsole.WriteLine("Press any key to continue.");
                AnsiConsole.Console.Input.ReadKey(true);
            }
    }

    public void ViewHabitDetails()
    {
        if (dataManager.Habits.Count > 0)
        {
            var habit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                .Title("Select habit to view")
                .AddChoices(dataManager.Habits)
            );
            AnsiConsole.WriteLine("Name: " + habit.Name);
            AnsiConsole.WriteLine("Score: " + habit.DisplayProgress());
            AnsiConsole.WriteLine("Good Habit: "+ habit.IsGoodHabit);
            AnsiConsole.WriteLine("Press any key to continue.");
            AnsiConsole.Console.Input.ReadKey(true);
        } else
        {
            AnsiConsole.Write("No habits to view");
        }
    }
    public void AddPoint()
    {
        var habit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                .Title("Select habit to remove")
                .AddChoices(dataManager.Habits)
            );
        habit.IncrementScore();
        dataManager.saveHabits(dataManager.Habits);
    }
}