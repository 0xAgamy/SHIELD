using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Shield.Options;

namespace Shield.Services
{
    public class EmailServices
    {
        private readonly EmailOptions _emailOptions;

        public EmailServices(EmailOptions emailOptions)
        {
            _emailOptions = emailOptions;
        }

        public void SendEmail(string email,string subject, string message) {

            var Email = new MimeMessage();
            Email.From.Add(MailboxAddress.Parse(_emailOptions.SenderEmail));
            Email.To.Add(MailboxAddress.Parse(email));
            Email.Subject = subject;
            
            Email.Body = new TextPart(TextFormat.Html) { Text = message} ;

            using var smtp = new SmtpClient();
            smtp.Connect(_emailOptions.Server,_emailOptions.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailOptions.SenderEmail, _emailOptions.Password);

            smtp.Send(Email);
            smtp.Dispose(); 
        
        } 

    }
}
