namespace Game;

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
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
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select Mode")
                .AddChoices("Add Habit", "Remove Habit", "Add Point", "Check Score", "View Habit Details", "Reset Score", "Quit")
        );


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
        if (mode == "Add Point")
        {
            AddPoint();
        }

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
        Habit newHabit = new Habit(Name:name, Goal:goal, Score:0, IsGoodHabit:goodBad);
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
                .Title("Select habit to remove")
                .AddChoices(dataManager.Habits)
            );
            AnsiConsole.WriteLine("Name: "+ habit.Name);
            AnsiConsole.WriteLine("Score: " + habit.DisplayProgress());
            AnsiConsole.WriteLine("Good Habit: "+ habit.IsGoodHabit);
        } else
        {
            AnsiConsole.Write("No habits to remove");
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