namespace Game.Test;

using Game;
using System.Text.Json;

public class FileSaverTests
{
    FileSaver testFile;
    string testName = Path.GetTempFileName();
    List<Habit> Habits;
    public FileSaverTests()
    {
        testFile = new FileSaver(testName);
    }

    [Fact]
    public void TestSave()
    {
        Habit testHabit = new Habit("test habit", 10, 0, true);
        Habits = new List<Habit>([testHabit]);
        var result = testFile.Save(Habits);
        Assert.True(result);
        var json = File.ReadAllText(testName);
        var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
        var saved = JsonSerializer.Deserialize<List<Habit>>(json, options);
        Assert.NotNull(saved);
        Assert.Equal(testHabit.Name,saved[0].Name);

    }
}
