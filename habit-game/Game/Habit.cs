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
            Console.WriteLine("Goal must be no less than 0.");
        }
    }

    public string DisplayProgress()
    {
        var progress = this.Score + "/" + this.Goal;
        return progress;
    }

    public void IncrementScore()
    {
        this.Score ++;
    }
}

