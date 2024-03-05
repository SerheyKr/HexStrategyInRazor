using System.Net.Mail;

namespace WebApplication1.EmailService
{
    public static class EmailService
    {
        public static void SendMessageToEmail(string email, string message, string header)
        {
            throw new NotImplementedException();

            Console.WriteLine(EmailServiceSettings.Instance.EmailForSendingMessages);

            MailMessage mes = new MailMessage(EmailServiceSettings.Instance.EmailForSendingMessages, email);
            mes.Subject = header;
            mes.Body = message;
            SmtpClient client = new SmtpClient();

            client.UseDefaultCredentials = true;

            client.Send(mes);
        }
    }
}
