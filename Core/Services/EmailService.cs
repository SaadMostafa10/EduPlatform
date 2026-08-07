using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Services.Abstractions;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailOptions.DisplayName ?? "EduPlatform", _emailOptions.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
