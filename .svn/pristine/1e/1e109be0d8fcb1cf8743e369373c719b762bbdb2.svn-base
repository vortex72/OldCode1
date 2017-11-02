using System.Net.Mail;

namespace EPWI.Components.Utility
{
    public class MailUtility
    {
        public static void SendEmail(string fromAddress, string toAddress, string subject, string body)
        {
            var sc = new SmtpClient();
            var m = new MailMessage();
            sc.Send(fromAddress, toAddress, subject, body);
        }

        public static void SendEmail(MailMessage message)
        {
            var sc = new SmtpClient();
            message.ReplyToList.Clear();
            message.ReplyToList.Add(message.From);
            message.From = new MailAddress("admin@epwi.net");
            sc.Send(message);
        }
    }
}