﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Email.Options;
using OnAim.Admin.APP.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OnAim.Admin.APP.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly ILogger<SendGridEmailSender> _logger;
        private readonly EmailOptions _config;

        public SendGridEmailSender(IOptions<EmailOptions> emailConfig, ILogger<SendGridEmailSender> logger)
        {
            _logger = logger;
            _config = emailConfig.Value;
        }

        private SendGridClient SendGridClient => new(_config.SendGridOptions?.ApiKey);

        public async Task SendAsync(EmailObject emailObject)
        {
            emailObject.NotBeNull();

            var message = new SendGridMessage { Subject = emailObject.Subject, HtmlContent = emailObject.MailBody, };

            message.AddTo(new EmailAddress(emailObject.ReceiverEmail));

            message.From = new EmailAddress(emailObject.SenderEmail ?? _config.From);
            message.ReplyTo = new EmailAddress(emailObject.SenderEmail ?? _config.From);

            await SendGridClient.SendEmailAsync(message);

            _logger.LogInformation(
                "Email sent. From: {From}, To: {To}, Subject: {Subject}, Content: {Content}",
                _config.From,
                emailObject.ReceiverEmail,
                emailObject.Subject,
                emailObject.MailBody
            );
        }
    }
}
