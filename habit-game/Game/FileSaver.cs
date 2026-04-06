namespace Game;

using System.IO;
using System.Text.Json;

public class FileSaver {
    string fileName;

    public FileSaver(string fileName) {
        this.fileName = fileName;
        if(!File.Exists(this.fileName)) {
            File.WriteAllText(this.fileName, "[]");
        }
    }

    public bool Save<T>(List<T> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };
        string serialized = JsonSerializer.Serialize(data, options);
        File.WriteAllText(fileName, serialized);
        return true;
    }
}
