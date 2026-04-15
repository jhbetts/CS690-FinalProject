using System.Net;
using System.Net.Mail;
using System.Text.Json.Serialization;

public class MailService
{
    public string serverName {get; set;}
    public string fromUsername {get; set;}
    public string fromPassword {get; set;}
    public int port {get; set;}
    public bool SSLRequired {get; set;}
    [JsonConstructor]

    public MailService(
        string serverName, 
        string fromUsername, 
        string fromPassword, 
        int port,
        bool SSLRequired
    )
    {
        this.serverName = serverName;
        this.fromUsername = fromUsername;
        this.fromPassword = fromPassword;
        this.port = port;
        this.SSLRequired = SSLRequired;
    }
    public void SendAlert(string body, string toUsername)
    {
        try
        {

        SmtpClient mySmtpClient = new SmtpClient(serverName, port);

        mySmtpClient.UseDefaultCredentials = false;
        NetworkCredential basicAuthenticationInfo = new NetworkCredential(fromUsername, fromPassword);
        mySmtpClient.Credentials = basicAuthenticationInfo;
        mySmtpClient.EnableSsl = SSLRequired;

        MailAddress from = new MailAddress(fromUsername, "Habit-Game");
        MailAddress to = new MailAddress(toUsername);
        MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

        MailAddress replyTo = new MailAddress(fromUsername);
        myMail.ReplyToList.Add(replyTo);

        myMail.Subject = "Habit-Game Progress Report";
        myMail.SubjectEncoding = System.Text.Encoding.UTF8;

        myMail.Body = body;
        myMail.BodyEncoding = System.Text.Encoding.UTF8;
        myMail.IsBodyHtml = true;

        mySmtpClient.Send(myMail);
        }

        catch (SmtpException ex)
        {
        throw new ApplicationException
            ("SmtpException has occured: " + ex.Message);
        }
        catch (Exception ex)
        {
        throw ex;
        }
        
    }
}

