using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OnAim.Admin.APP.Email.Options;

namespace OnAim.Admin.APP.Email
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEmailService(
            this IServiceCollection services,
            IConfiguration configuration,
            EmailProvider provider = EmailProvider.MimKit,
            Action<EmailOptions>? configureOptions = null
        )
        {
            services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));

            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            if (provider == EmailProvider.SendGrid)
            {
                services.TryAddSingleton<IEmailSender, SendGridEmailSender>();
            }
            else
            {
                services.TryAddSingleton<IEmailSender, MailKitEmailSender>();
            }

            return services;
        }
    }
}
