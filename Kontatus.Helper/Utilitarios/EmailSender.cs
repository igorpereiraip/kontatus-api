using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ConsigIntegra.Helper.Utilitarios
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string assunto, string email, IEnumerable<string> destinatarios, IEnumerable<string> anexos = null, string replyTo = null);
    }

    public class EmailSender : IEmailSender
    {
        private readonly int port;
        private readonly string host;
        private readonly string from;
        private readonly string username;
        private readonly string password;

        public EmailSender(string host, int port, string from, string username, string password)
        {
            this.host = host;
            this.port = port;
            this.from = from;
            this.username = username;
            this.password = password;
        }

        public async Task SendEmailAsync(string assunto, string email, IEnumerable<string> destinatarios, IEnumerable<string> anexos = null, string replyTo = null)
        {
            if (destinatarios is null || destinatarios.Count() <= 0)
                return;

            using var message = new MailMessage
            {
                Subject = assunto,
                Body = email,
                From = new MailAddress(from),
                IsBodyHtml = true,
            };

            using var client = new SmtpClient(host, port)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password),
            };            

            if (replyTo != null)
            {
                message.ReplyToList.Add(new MailAddress(replyTo));
            }

            foreach (var r in destinatarios)
            {
                message.To.Add(r);
            }

            if (anexos != null)
            {
                foreach (var a in anexos)
                {
                    message.Attachments.Add(new Attachment(a));
                }
            }

            await client.SendMailAsync(message);
        }
    }
}
