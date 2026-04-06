namespace Game.Test;

using Game;

public class ScoreTests
{
    Score testScore;
    int goal = 10;
    int progress = 5;
    public ScoreTests()
    {
        testScore = new Score(goal,progress);
    }

    [Fact]
    public void TestReset()
    {
        testScore.ResetScore();
        Assert.Equal(0, testScore.Progress);

    }
    [Fact]
    public void TestIncrement()
    {
        int initialScore = testScore.Progress;
        testScore.IncrementScore();
        Assert.Equal(initialScore+1, testScore.Progress);
    }
}
