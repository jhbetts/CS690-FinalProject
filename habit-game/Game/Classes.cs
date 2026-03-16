using System.Text.Json.Serialization;

namespace Game;

public class Habit
{
    public string Name{get; set;}
    public int Goal{get; set;}
    public bool IsGoodHabit{get; set;}
    public int Score{get; set;}

    [JsonConstructor]
    public Habit(string Name, int Goal, int Score, bool IsGoodHabit)
    {
        this.Name = Name;
        this.Goal = Goal;
        this.IsGoodHabit = IsGoodHabit;
        this.Score = Score;
    }

    public override string ToString()
    {
        return Name;
    }
    public void DisplayDetails()
    {
        Console.WriteLine(Name);
        Console.WriteLine("Goal:" + Goal);
        Console.WriteLine("Good habit: "+IsGoodHabit);
    }

    public void SetGoal(int goalnum)
    {   
        if(goalnum >= 0)
        {
            Goal = goalnum;
        } else
        {
            Goal = 0;
            Console.WriteLine("Goal must be no less than 0. If this is a bad habit, ");
        }
    }

    public string DisplayProgress()
    {
        var progress = Math.Abs(this.Score) + "/" + this.Goal;
        return progress;
    }

    public void IncrementScore()
    {
        if (this.IsGoodHabit)
        {
            this.Score ++;  
        } else
        {
            this.Score --;
        }
    }
}

// public class GoodHabit : Habit
// {
//     public GoodHabit(string Name, int Goal, int Score, bool IsGoodHabit)
//         : base(Name, Goal, Score, IsGoodHabit)
//     {

//     }
//     public void IncrementScore()
//     {
//         this.Score ++;
//     }
// }
// public class BadHabit : Habit
// {
//     public BadHabit(string Name, int Goal, int Score, bool IsGoodHabit)
//         : base(Name, Goal, Score, IsGoodHabit)
//     {
//         this.Goal *= -1;
//     }
//     public void IncrementScore()
//     {
//         this.Score --;
//     }
// }

public class Score
{
    public DateTime Date{get;set;}
    public int Goal{get; set;}
    public int Progress{get; set;}

    [JsonConstructor]
    public Score(int Goal, int Progress)
    {
        this.Goal = Goal;
        this.Progress = Progress;
        this.Date = DateTime.Now;
    }

    public override string ToString()
    {
        var progress = this.Progress + "/" + this.Goal;
        return "Today's Score: " + progress;
    }

    public void ResetScore()
    {
        this.Progress = 0;
    }

    public void IncrementScore()
    {
        this.Progress ++;
    }
}

