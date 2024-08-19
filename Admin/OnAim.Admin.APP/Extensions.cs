using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.APP.Commands.User.Create;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Factory;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using System.Reflection;
using FluentValidation;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.APP.Commands.Role.Create;
using OnAim.Admin.APP.Queries.Role.GetAll;
using OnAim.Admin.APP.Commands.Role.Update;
using OnAim.Admin.APP.Queries.Role.GetById;
using OnAim.Admin.APP.Commands.EndpointGroup.Create;
using OnAim.Admin.APP.Commands.EndpointGroup.Update;
using OnAim.Admin.APP.Queries.EndpointGroup.GetAll;
using OnAim.Admin.APP.Queries.EndpointGroup.GetById;
using OnAim.Admin.APP.Commands.EndPoint.Create;
using OnAim.Admin.APP.Commands.EndPoint.Disable;
using OnAim.Admin.APP.Commands.EndPoint.Enable;
using OnAim.Admin.APP.Commands.EndPoint.Update;

namespace OnAim.Admin.APP
{
    public static class Extensions
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

            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateRoleCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllRolesQueryHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateRoleCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetRoleByIdQueryHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEndpointGroupCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateEndpointGroupCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEndpointGroupQueryHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetEndpointGroupByIdQueryHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEndpointCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DisableEndpointCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EnableEndpointCommandHandler).Assembly))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateEndpointCommandHandler).Assembly));

            services.AddTransient<IRequestHandler<CreateUserCommand, ApplicationResult>, CreateUserCommandHandler>();
            services.AddTransient<IRequestHandler<LoginUserCommand, AuthResultDto>, LoginUserCommandHandler>();

            services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();


            return services;
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> src)
        => src ?? Enumerable.Empty<T>();

    }
}
