namespace Game;

using System.Text.Json;
public class DataManager
{
    FileSaver habitSaver;
    FileSaver scoreSaver;
    public List<Habit> Habits;
    public List<Score> Scores;
    public Score TodaysScore;

    public DataManager()
    {
        var HabitFile = "./habit_data.json";
        var ScoreFile = "./scores.json";
        habitSaver = new FileSaver(HabitFile);
        scoreSaver = new FileSaver(ScoreFile);
        if (File.Exists(HabitFile))
        {        
            var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };

            string json = File.ReadAllText(HabitFile); 
            Habits = JsonSerializer.Deserialize<List<Habit>>(json, options)!;        
        } else
        {
            Habits = new List<Habit>();
        }
        if (File.Exists(ScoreFile))
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            };
            string json = File.ReadAllText(ScoreFile);
            Scores = JsonSerializer.Deserialize<List<Score>>(json,options)!;
        } else
        {
            Scores = new List<Score>();
        }
        TodaysScore = new Score(0,0);
        SetTodaysScore(TodaysScore, Habits);

    }
    
    public void ResetScores(List<Habit> habits)
    {
        for(int i = 0; i < habits.Count; i++)
        {
            var habit = habits[i];
            habit.Score = 0;
        }
        saveHabits(habits);
        SetTodaysScore(TodaysScore, habits);
    }

    public void SetTodaysScore(Score score, List<Habit> habits)
    {
        score.Goal = 0;
        score.Progress = 0;
        for(int i = 0; i < habits.Count; i++)
        {
            var habit = habits[i];
            var goal = habit.Goal;
            var progress = habit.Score;
            if (habit.IsGoodHabit == true)
            {
                score.Goal += goal;
                score.Progress += progress;                
            } else
            {
                score.Goal += goal; 
                score.Progress += goal+progress;
            }
        }
    }

    public void addHabit(Habit habit)
    {   
        Habits.Add(habit);
        habitSaver.Save(Habits);
        SetTodaysScore(TodaysScore, Habits);
    }
    public void removeHabit(Habit habit)
    {   
        Habits.Remove(habit);
        habitSaver.Save(Habits);
        SetTodaysScore(TodaysScore, Habits);

    }
    public void saveHabits(List<Habit> habits)
    {
        habitSaver.Save(Habits);
        SetTodaysScore(TodaysScore, Habits);

    }
    public void saveScore(List<Score> scores)
    {

        scoreSaver.Save(scores);
    }
}