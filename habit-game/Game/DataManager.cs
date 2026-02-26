namespace Game;

using System.Runtime.CompilerServices;
using System.Text.Json;
public class DataManager
{
    FileSaver fileSaver;
    public List<Habit> Habits;

    public DataManager()
    {
        var filepath = "/workspaces/CS690-FinalProject/habit-game/Game/habit_data.json";
        fileSaver = new FileSaver(filepath);
        if (File.Exists(filepath))
        {        
            var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };

            string json = File.ReadAllText(filepath); 
            Habits = JsonSerializer.Deserialize<List<Habit>>(json, options)!;        
        } else
        {
            Habits = new List<Habit>();
        }
    }

    public void addHabit(Habit habit)
    {   
        Habits.Add(habit);
        fileSaver.SaveHabits(Habits);
    }
    public void removeHabit(Habit habit)
    {   
        Habits.Remove(habit);
        fileSaver.SaveHabits(Habits);
    }
    public void saveHabits(List<Habit> habits)
    {
        fileSaver.SaveHabits(Habits);
    }
}