// Source - https://stackoverflow.com/a/449897
// Posted by splattne, modified by community. See post 'Timeline' for change history
// Retrieved 2026-04-13, License - CC BY-SA 4.0

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

            // set smtp-client with basicAuthentication
            mySmtpClient.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new NetworkCredential(fromUsername, fromPassword);
        mySmtpClient.Credentials = basicAuthenticationInfo;
        mySmtpClient.EnableSsl = SSLRequired;

        // add from,to mailaddresses
        MailAddress from = new MailAddress(fromUsername, "Habit-Game");
        MailAddress to = new MailAddress(toUsername);
        MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

        // add ReplyTo
        MailAddress replyTo = new MailAddress(fromUsername);
        myMail.ReplyToList.Add(replyTo);

        // set subject and encoding
        myMail.Subject = "Habit-Game Progress Report";
        myMail.SubjectEncoding = System.Text.Encoding.UTF8;

        // set body-message and encoding
        myMail.Body = body;
        myMail.BodyEncoding = System.Text.Encoding.UTF8;
        // text or html
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

