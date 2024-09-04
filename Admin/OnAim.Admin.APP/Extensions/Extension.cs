using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.APP.Factory;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.APP.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OnAim.Admin.APP.Email;
using OnAim.Admin.APP.Email.Options;

namespace OnAim.Admin.APP.Extensions
{
    public static class Extension
    {
        public static IServiceCollection AddApp(
            this IServiceCollection services,
            IConfiguration configuration,
            EmailProvider provider = EmailProvider.MimKit,
            Action<EmailOptions>? configureOptions = null)
        {
            //var config = configuration.BindOptions<EmailOptions>(nameof(EmailOptions));
            //configureOptions?.Invoke(config);

            services
                .AddScoped<IEndpointRepository, EndpointRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddTransient<IJwtFactory, JwtFactory>();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IRejectedLogRepository, RejectedLogRepository>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IDomainValidationService, DomainValidationService>();
            if (provider == EmailProvider.SendGrid)
            {
                services.TryAddSingleton<OnAim.Admin.APP.Email.IEmailSender, SendGridEmailSender>();
            }
            else
            {
                services.TryAddSingleton<OnAim.Admin.APP.Email.IEmailSender, MailKitEmailSender>();
            }
            //services.AddHostedService<RejectedLogRetryService>();

            services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();

            var serviceProvider = services.BuildServiceProvider();

            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> src)
        => src ?? Enumerable.Empty<T>();
    }
}
