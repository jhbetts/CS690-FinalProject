using System.Text.Json.Serialization;

namespace Game;

public class Alert
{
    public string From{get;set;}
    public string To{get; set;}
    public string Text{get; set;}

    [JsonConstructor]
    public Alert(string From, string To, string Text)
    {
        this.From = From;
        this.To = To;
        this.Text = Text;
    }

}