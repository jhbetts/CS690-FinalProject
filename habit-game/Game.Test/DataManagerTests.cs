namespace Game.Test;

using Game;
using System.Text.Json;

public class DataManagerTests
{
    DataManager testDataManager;
    public List<Habit> testHabits;
    public List<Score> testScores;
    public Score testTodaysScore;
    Habit testHabit = new Habit("test habit", 10, 0, true);

    public DataManagerTests()
    {
        testDataManager = new DataManager();
        testHabits = new List<Habit>([testHabit]);
        testScores = new List<Score>();
        testTodaysScore = new Score(0,0);
        testDataManager.Habits = testHabits;
        testDataManager.Scores = testScores;
        testDataManager.TodaysScore = testTodaysScore;
    }

    [Fact]
    public void TestAddHabit()
    {
        Habit newTestHabit = new Habit(" new test habit", 10, 0, true);
        var result = testDataManager.addHabit(newTestHabit);
        Assert.True(result);
    }
    [Fact]
    public void TestRemoveHabit()
    {
        testDataManager.removeHabit(testHabit);
        Assert.True(testDataManager.Habits.Count() == 0);
    }

    [Fact]
    public void TestSaveHabits()
    {
        string testFile = Path.GetTempFileName();
        File.WriteAllText(testFile,"[]");

        testDataManager.habitSaver = new FileSaver(testFile);

        var habitsToSave = new List<Habit>
        {
            new Habit("Test 1", 10, 0, true),
            new Habit("Test 2", 5, 5, false)
        };

        testDataManager.Habits = habitsToSave;
        testDataManager.Scores = new List<Score>();
        testDataManager.TodaysScore = new Score(0,0);
        var result = testDataManager.saveHabits(testDataManager.Habits);
        Assert.True(result);
        Assert.True(File.Exists(testFile));

        var json = File.ReadAllText(testFile);
        var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
        var saved = JsonSerializer.Deserialize<List<Habit>>(json, options);
        Assert.NotNull(saved);
        Assert.Equal(habitsToSave.Count, saved.Count);
        File.Delete(testFile);
    }
    [Fact]
    public void TestSaveScore()
    {
        string testFile = Path.GetTempFileName();
        File.WriteAllText(testFile,"[]");

        testDataManager.scoreSaver = new FileSaver(testFile);

        var testScore = new Score(0,0);

        testDataManager.TodaysScore = new Score(0,0);
        testDataManager.Scores = [testDataManager.TodaysScore];
        var result = testDataManager.saveScore(testDataManager.Scores);
        Assert.True(result);
        Assert.True(File.Exists(testFile));

        var json = File.ReadAllText(testFile);
        var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
        var saved = JsonSerializer.Deserialize<List<Score>>(json, options);
        Assert.NotNull(saved);
        Assert.Equal(testScore.Goal, saved[0].Goal);
        Assert.Equal(testScore.Progress, saved[0].Progress);
        File.Delete(testFile);
    }

}