using System.Text.Json.Serialization;

namespace Game;

public class Alert
{
    public string To{get; set;}
    public DateTime SendTime{get; set;}

    [JsonConstructor]
    public Alert( string To, DateTime SendTime)
    {
        this.To = To;
        this.SendTime=SendTime;
    }

}