using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Authentication;
using System.Net.Mail;


namespace Asp.net_recepy_blog.Pages.Services
{
    public class EmailServices
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailServices(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;

        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WebTaskMenager", _username));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                //await client.EnableSsl = true;
                await client.ConnectAsync(_host, _port, true);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
