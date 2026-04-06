namespace Game.Test;
using Game;
public class HabitTests
{
    Habit goodTestHabit;
    string testName = "Test Habit";
    int goal = 10;
    int score = 0;

    public HabitTests()
    {
        goodTestHabit = new Habit(testName,goal, score, true);
    }

    [Fact]
    public void TestOccurrence()
    {
        var startingScore = goodTestHabit.Score;
        goodTestHabit.IncrementScore();
        var result = goodTestHabit.Score;
        Assert.Equal(startingScore+1, result);
    }
}
