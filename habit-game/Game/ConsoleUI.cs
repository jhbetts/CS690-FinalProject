namespace Game;

using System.Net.Mail;
using Spectre.Console;

public class ConsoleUI
{
    DataManager dataManager;

    public ConsoleUI()
    {
        dataManager = new DataManager();
    }
    public void Show()
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
                        "Create Alerts",
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
            if (mode == "Create Alerts")
            {
                CreateAlert();
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

    private void CreateAlert()
    {
        Console.WriteLine("Alerts are not functional at this time.");
        string fromEmail = AnsiConsole.Ask<string>("From: ");
        string toEmail = AnsiConsole.Ask<string>("To: ");
        string text = "";
        DateTime alertTime;
        string prompt = "Enter time to send alert email in 24 hour format (e.g. 22:00): ";
        Console.WriteLine(prompt);
        string time = Console.ReadLine();
        while(!DateTime.TryParse(time, out alertTime))
        {
            Console.WriteLine("Time invalid, try again.");
            Console.WriteLine(prompt);
            time = Console.ReadLine();
        }


        for (int i = 0; i < dataManager.Habits.Count(); i++)
        {
            string line = dataManager.Habits[i].Name + "|" + dataManager.Habits[i].DisplayProgress() + "|" + dataManager.Habits[i].GoodBad() + "\n";
            text += line;
        }
        var alert = new Alert(From: fromEmail, To: toEmail, Text: text);
    }
    private void CheckScore(Score score)
    {
        var table = new Table();

        // Columns
        table.AddColumn("Name");
        table.AddColumn("Score");
        table.AddColumn("Good/Bad");
        // table.AddRow("Total Score", score.ToString, "");
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
            AnsiConsole.WriteLine("Name: "+ habit.Name);
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