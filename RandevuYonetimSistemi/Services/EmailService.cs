using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Configuration;

namespace RandevuYonetimSistemi.Services
{
    public class EmailService
    {
        public void SendVerificationCode(string toEmail, string code)
        {
            var fromEmail = ConfigurationManager.AppSettings["EmailAddress"];
            var smtpKey = ConfigurationManager.AppSettings["BrevoApiKey"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Appointment", fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Email verification";

            message.Body = new TextPart("plain")
            {
                Text =
                    "Hello,\n\n" +
                    "Your verification code: " + code + "\n\n" 
            };

        
        }
    }
}
