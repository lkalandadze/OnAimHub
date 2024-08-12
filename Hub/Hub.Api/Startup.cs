using Consul;
using Hub.Api.Consul;
using Hub.Api.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Lib;
using System.Text;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var jwtSettings = Configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                ValidAudience = jwtSettings.GetValue<string>("Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        if (IsRunningInDocker())
        {
            services.Configure<ConsulConfig>(Configuration.GetSection("Consul"));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(Configuration["Consul:Host"]);
            }));

            services.AddHostedService<ConsulHostedService>();
        }

        services.AddControllers();

        services.AddAuthorization();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        });

        if (IsRunningInDocker())
        {

            lifetime.ApplicationStarted.Register(() =>
            {
                var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
                var registration = new AgentServiceRegistration()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "hubapi",
                    Address = "hubapi", // Docker service name or external IP address
                    Port = 8080 // The port your service is running on inside the container
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            // Deregister the service from Consul when application stops
            lifetime.ApplicationStopped.Register(() =>
            {
                var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
                var registration = new AgentServiceRegistration()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "hubapi"
                };

                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    private bool IsRunningInDocker()
    {
        var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
        return !string.IsNullOrEmpty(isDocker) && isDocker == "true";
    }
}