namespace Game;

using System.IO;
using System.Text.Json;

public class FileSaver {
    string fileName;

    public FileSaver(string fileName) {
        this.fileName = fileName;
        if(!File.Exists(this.fileName)) {
            File.Create(this.fileName).Close();
        }
    }

    // public void AppendLine(string line) {
    //     File.AppendAllText(this.fileName, line + Environment.NewLine);
    // }

    public void SaveHabits(List<Habit> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };
        string serialized = JsonSerializer.Serialize(data, options);
        File.WriteAllText(fileName, serialized);
    }
}