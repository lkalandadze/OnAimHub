using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OnAim.Admin.APP.Commands.User.Login;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using System.Reflection;
using System.Text;
using OnAim.Admin.APP.Commands.User.Create;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.APP.Factory;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services;
using OnAim.Admin.APP.Commands.EndPoint.Create;
using OnAim.Admin.APP.Commands.EndPoint.Disable;
using OnAim.Admin.APP.Commands.EndPoint.Enable;
using OnAim.Admin.APP.Commands.EndPoint.Update;
using OnAim.Admin.APP.Commands.EndpointGroup.Create;
using OnAim.Admin.APP.Commands.EndpointGroup.Update;
using OnAim.Admin.APP.Queries.EndpointGroup.GetAll;
using OnAim.Admin.APP.Queries.EndpointGroup.GetById;
using OnAim.Admin.APP.Queries.Role.GetAll;
using OnAim.Admin.APP.Queries.Role.GetById;
using OnAim.Admin.APP.Commands.Role.Create;
using OnAim.Admin.APP.Commands.Role.Update;
using OnAim.Admin.APP.Commands.User.Update;
using OnAim.Admin.API.Service;
using OnAim.Admin.APP.Models.Response.User;

namespace OnAim.Admin.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));
            });


            return services;
        }
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            return services;
        }
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(configuration["JwtConfiguration:Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JwtConfiguration:Issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["JwtConfiguration:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = configuration["JwtConfiguration:Issuer"];
                options.Audience = configuration["JwtConfiguration:Audience"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration["JwtConfiguration:Issuer"];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            return services;
        }
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            return services;
        }
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            return services;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEndpointGroupRepository, EndpointGroupRepository>();
            services.AddScoped<IEndpointRepository, EndpointRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEndpointService, EndpointService>();
            services.AddTransient<IJwtFactory, JwtFactory>();

            return services;
        }
        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();

            return services;
        }
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateRoleCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllRolesQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateRoleCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetRoleByIdQueryHandler).Assembly));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEndpointGroupCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateEndpointGroupCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEndpointGroupQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetEndpointGroupByIdQueryHandler).Assembly));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEndpointCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DisableEndpointCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EnableEndpointCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateEndpointCommandHandler).Assembly));



            services.AddTransient<IRequestHandler<CreateUserCommand, ApplicationResult>, CreateUserCommandHandler>();
            services.AddTransient<IRequestHandler<LoginUserCommand, AuthResultDto>, LoginUserCommandHandler>();

            return services;
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }
    }
}
