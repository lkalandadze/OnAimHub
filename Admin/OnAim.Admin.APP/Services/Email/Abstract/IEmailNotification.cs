﻿namespace OnAim.Admin.APP.Services.Email.Abstract;

public interface IEmailNotification
{
    Task SendAsync(IEmailMessage emailMessage, CancellationToken cancellationToken = default);
}

public interface IEmailMessage
{
    public string From { get; set; }

    public string Tos { get; set; }

    public string CCs { get; set; }

    public string BCCs { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}