using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using LevelService.Api.Extensions;
using LevelService.Application;
using LevelService.Application.Services.Abstract;
using LevelService.Application.Services.Abstract.BackgroundJobs;
using LevelService.Application.Services.Concrete;
using LevelService.Application.Services.Concrete.BackgroundJobs;
using LevelService.Infrastructure;
using LevelService.Infrastructure.DataAccess;
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
            .AddInfrastructureLayer(configuration)
            .AddMassTransitWithRabbitMqTransport(configuration, consumerAssemblyMarkerType: typeof(Program))
            .AddCustomServices(configuration);


//needs to be taken to custom services
builder.Services.AddScoped<IStageSchedulerService, StageSchedulerService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(configuration.GetConnectionString("OnAimLevel")));
builder.Services.AddHangfireServer();

//builder.Services.AddHostedService<JobSyncService>();

CustomServiceExtensions.ConfigureJwt(builder.Services, configuration);
CustomServiceExtensions.ConfigureSwagger(builder.Services);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

var origins = builder.Configuration.GetValue<string>("OriginsToAllow");

builder.Services.AddCors();


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var serviceProvider = builder.Services.BuildServiceProvider();

    var context = serviceProvider.GetService<LevelDbContext>();

    if (context != null)
    {
        DbInitializer.InitializeDatabase(app.Services, context);
    }


    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseHangfireDashboard();

    app.Run();
}