using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Leaderboard.Api.Extensions;
using Leaderboard.Application;
using Leaderboard.Application.Services.Abstract;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Application.Services.Concrete;
using Leaderboard.Application.Services.Concrete.BackgroundJobs;
using Leaderboard.Infrastructure;
using Leaderboard.Infrastructure.DataAccess;
using MassTransit;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining(typeof(Program));
    fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("ka-GE");
    fv.ValidatorOptions.LanguageManager.Enabled = true;
    fv.LocalizationEnabled = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services
            .AddLocalization()
            .AddEndpointsApiExplorer();

builder.Services
            .AddApplicationLayer()
            .AddMassTransitWithRabbitMqTransport(configuration, consumerAssemblyMarkerType: typeof(Program))
            .AddInfrastructureLayer(configuration)
            .AddCustomServices(configuration);

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

//needs to be taken to custom services
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IBackgroundJobScheduler, BackgroundJobScheduler>();

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(configuration.GetConnectionString("OnAimLeaderboard")));
builder.Services.AddHangfireServer();

builder.Services.AddHostedService<JobSyncService>();
builder.Services.AddHostedService<LeaderboardStatusUpdaterService>();
builder.Services.AddHostedService<LeaderboardScheduleStatusUpdaterService>();

CustomServiceExtensions.ConfigureJwt(builder.Services, configuration);
CustomServiceExtensions.ConfigureSwagger(builder.Services);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var origins = builder.Configuration.GetValue<string>("OriginsToAllow");

builder.Services.AddCors();

var app = builder.Build();
{
    //if (app.Environment.IsDevelopment())
    //{
    //}
    app.UseSwagger();
    app.UseSwaggerUI();

    var serviceProvider = builder.Services.BuildServiceProvider();

    var context = serviceProvider.GetService<LeaderboardDbContext>();

    if (context != null)
    {
        DbInitializer.InitializeDatabase(app.Services, context);
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new AllowAllAuthorizationFilter() }
    });

    app.Run();

}

public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; // Allow all access
    }
}