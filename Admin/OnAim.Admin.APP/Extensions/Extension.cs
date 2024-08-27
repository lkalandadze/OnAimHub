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
using Microsoft.AspNetCore.Http;

namespace OnAim.Admin.APP.Extensions
{
    public static class Extension
    {
        public static IServiceCollection AddApp(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IEndpointGroupRepository, EndpointGroupRepository>()
                .AddScoped<IEndpointRepository, EndpointRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddTransient<IJwtFactory, JwtFactory>();

            services.AddHttpContextAccessor();

            var serviceProvider = services.BuildServiceProvider();
            HttpContextAccessorProvider.HttpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static class HttpContextAccessorProvider
        {
            public static IHttpContextAccessor HttpContextAccessor { get; set; }
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> src)
        => src ?? Enumerable.Empty<T>();

    }
}
