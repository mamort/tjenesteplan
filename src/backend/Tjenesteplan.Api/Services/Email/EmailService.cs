using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Tjenesteplan.Api.Configuration;
using Tjenesteplan.Domain;
using Tjenesteplan.Domain.Api.Email;

namespace Tjenesteplan.Api.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly CommonOptions _commonOptions;
        private const string PickupDirectory = "Emails-Pickup";
        private readonly string _rootPath;
        private readonly Lazy<SendGridClient> _client;

        private const string FromAddress = "matsmortensen@gmail.com";

        public EmailService(
            IWebHostEnvironment hostingEnvironment,
            IOptions<CommonOptions> commonOptions,
            IOptions<SendGridOptions> sendGridOptions,
            ILogger<EmailService> _logger
        )
        {
            this._logger = _logger;
            _commonOptions = commonOptions.Value;
            _rootPath = hostingEnvironment.ContentRootPath;
            _client = new Lazy<SendGridClient>(() => new SendGridClient(sendGridOptions.Value.ApiKey));
        }

        public void Send(EmailMessage email)
        {
            SendAsync(email).GetAwaiter().GetResult();
        }

        public Task SendAsync(EmailMessage email)
        {
            if (_commonOptions.Environment == TjenesteplanEnvironment.Local)
            {
                SendLocalEmail(email);
                return Task.CompletedTask;
            }

            try
            {
                return SendEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to send email {email.GetType().Name} to {email.Email}", e);
                throw;
            }
        }

        private async Task SendEmailAsync(EmailMessage email)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress(FromAddress, "Tjenesteplan"));

            var recipients = new List<EmailAddress>
            {
                new EmailAddress(email.Email)
            };
            msg.AddTos(recipients);

            msg.SetSubject(email.Title);
            msg.AddContent(MimeType.Html, email.Body);

            var response = await _client.Value.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var body = "";
                try
                {
                    body = await response.Body.ReadAsStringAsync();
                }
                catch {  /* Ignore */ }

                throw new Exception("Unable to send email due to a SendGrid error. " +
                                    $"Statuscode: {response.StatusCode}, " +
                                    $"Body: {body}");
            }
            
        }

        private void SendLocalEmail(EmailMessage email)
        {
            if (!Directory.Exists(PickupDirectory))
            {
                Directory.CreateDirectory(PickupDirectory);
            }

            using (var client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = Path.Combine(_rootPath, PickupDirectory);

                var msg = new MailMessage(
                    from: FromAddress,
                    to: email.Email,
                    subject: email.Title,
                    body: email.Body
                );
                msg.IsBodyHtml = true;
                client.Send(msg);
            }
        }
    }
}