namespace Game;

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
                        // "View Score Detials", 
                        "View Habit Details", 
                        // "Reset Score", 
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
            if (mode == "View Habit Details")
            {
                ViewHabitDetails();
            }
            if (mode == "Add Occurrence")
            {
                AddPoint();
            }
            // if (mode == "Reset Score")
            // {
            //     dataManager.ResetScores(dataManager.Habits);
            // }
            if (mode == "Check Score")
            {
                CheckScore(score);
            }
            if (mode == "Quit")
            {
                dataManager.Scores.Add(score);
                dataManager.saveScore(dataManager.Scores);
                running = false;
            }
            AnsiConsole.Clear();
        }
    }

    private void CheckScore(Score score)
    {
        AnsiConsole.WriteLine(score.ToString());
        for (int i = 0; i < dataManager.Habits.Count(); i++)
        {
            AnsiConsole.WriteLine("Name: " + dataManager.Habits[i].Name);
            AnsiConsole.WriteLine("Score: " + dataManager.Habits[i].DisplayProgress());
        }
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