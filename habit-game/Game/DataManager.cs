namespace Game;

using System.Text.Json;
public class DataManager
{
    public FileSaver habitSaver;
    public FileSaver scoreSaver;
    public FileSaver alertSaver;
    public FileSaver serverSaver;
    public List<Habit> Habits;
    public List<Score> Scores;
    public List<Alert> Alerts;
    public Score TodaysScore;
    public List<MailService> Servers;

    public DataManager()
    {
        var HabitFile = "./habit_data.json";
        var ScoreFile = "./scores.json";
        var AlertFile = "./alerts.json";
        var ServerFile = "./server.json";
        serverSaver = new FileSaver(ServerFile);
        alertSaver = new FileSaver(AlertFile);
        habitSaver = new FileSaver(HabitFile);
        scoreSaver = new FileSaver(ScoreFile);
        if (File.Exists(ServerFile))
        {        
            var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };

            string json = File.ReadAllText(ServerFile); 
            Servers = JsonSerializer.Deserialize<List<MailService>>(json, options)!;        
        } else
        {
            Servers = new List<MailService>();
        }
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
        if (File.Exists(AlertFile))
        {        
            var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };

            string json = File.ReadAllText(AlertFile); 
            Alerts = JsonSerializer.Deserialize<List<Alert>>(json, options)!;        
        } else
        {
            Alerts = new List<Alert>();
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
    }
    public void AddMailservice(string mailServer, string mailUsername, string mailPassword, int mailPort, bool SSLRequired)
    {
        var newMailService = new MailService(mailServer, mailUsername, mailPassword, mailPort, SSLRequired);
        Servers.Add(newMailService);
        serverSaver.Save(Servers);

    }
    public void SendAlertEmails()
    {
        string text = "";
        for (int i = 0; i < Habits.Count(); i++)
        {
            string line = "<p>" + Habits[i].Name + " | " + Habits[i].DisplayProgress() + " | " + Habits[i].GoodBad() + "</p>";
            text += line;
        }


        for (int i = 0; i < Servers.Count(); i++)
        {
            var mailService = Servers[i];
            for (int j = 0; j < Alerts.Count(); j++)
            {
                var alert = Alerts[j];
                mailService.SendAlert(text, alert.To);
            }

        }


    }
    public void addAlert(Alert newAlert)
    {
        Alerts.Add(newAlert);
        alertSaver.Save(Alerts);
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

    public bool addHabit(Habit habit)
    {   
        Habits.Add(habit);
        habitSaver.Save(Habits);
        SetTodaysScore(TodaysScore, Habits);
        return true;
    }
    public bool removeHabit(Habit habit)
    {   
        Habits.Remove(habit);
        habitSaver.Save(Habits);
        SetTodaysScore(TodaysScore, Habits);
        return true;
    }
    public bool saveHabits(List<Habit> habits)
    {
        habitSaver.Save(habits);
        SetTodaysScore(TodaysScore, habits);
        return true;

    }
    public bool saveScore(List<Score> scores)
    {
        scoreSaver.Save(scores);
        return true;
    }
}