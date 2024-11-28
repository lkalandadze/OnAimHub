using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.APP.Services.Admin.EmailServices.Abstract;
using OnAim.Admin.APP.Services.Admin.EmailServices.SendGrid;
using OnAim.Admin.APP.Services.Admin.EmailServices.SmtpClient;

namespace OnAim.Admin.APP.Services.Admin.EmailServices;

public static class EmailNotificationServiceCollectionExtensions
{
    public static IServiceCollection AddSmtpClientEmailNotification(this IServiceCollection services, SmtpClientOptions options)
    {
        services.AddSingleton<IEmailNotification>(new SmtpClientEmailNotification(options));
        return services;
    }

    public static IServiceCollection AddSendGridEmailNotification(this IServiceCollection services, SendGridOptions options)
    {
        services.AddSingleton<IEmailNotification>(new SendGridEmailNotification(options));
        return services;
    }

    public static IServiceCollection AddEmailNotification(this IServiceCollection services, EmailOptions options)
    {
        if (options.UsedSmtpClient())
        {
            services.AddSmtpClientEmailNotification(options.SmtpClient);
        }
        else if (options.UsedSendGrid())
        {
            services.AddSendGridEmailNotification(options.SendGrid);
        }

        return services;
    }
}
