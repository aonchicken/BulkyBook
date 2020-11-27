using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Options;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            return Execute(_emailOptions.Server, _emailOptions.Port,
                _emailOptions.SenderEmail, _emailOptions.SenderName,
                _emailOptions.Username, _emailOptions.Password,
                subject, htmlMessage, email);

        }


        private static Task Execute(string server, int port,
            string senderEmail, string senderName,
            string username, string password,
            string subject, string htmlMessage, string email)
        {
            var message = new MailMessage {From = new MailAddress(senderEmail)};
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient {Host = server, Port = port};
            if (username == string.Empty) return smtp.SendMailAsync(message);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(username, password);
            return smtp.SendMailAsync(message);
        }
    }
}
