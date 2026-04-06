using System.Text.Json.Serialization;

namespace Game;


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
        this.Date = DateTime.Today;
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
