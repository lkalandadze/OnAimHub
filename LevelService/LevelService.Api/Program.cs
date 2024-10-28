using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using LevelService.Api.Extensions;
using LevelService.Application;
using LevelService.Infrastructure;
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
            .AddCustomServices(configuration);


//needs to be taken to custom services
//builder.Services.AddScoped<IJobService, JobService>();
//builder.Services.AddScoped<IBackgroundJobScheduler, BackgroundJobScheduler>();

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(configuration.GetConnectionString("OnAimLevelService")));
builder.Services.AddHangfireServer();

//builder.Services.AddHostedService<JobSyncService>();
//builder.Services.AddHostedService<LevelStatusUpdaterService>();

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

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseHangfireDashboard();

    app.Run();

}