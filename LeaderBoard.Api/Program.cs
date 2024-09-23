using FluentValidation.AspNetCore;
using Leaderboard.Api.Extensions;
using Leaderboard.Application;
using Leaderboard.Infrastructure;
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

builder.Services
            .AddLocalization()
            .AddEndpointsApiExplorer();

builder.Services
            .AddApplicationLayer()
            .AddInfrastructureLayer(configuration)
            .AddCustomServices(configuration);

CustomServiceExtensions.ConfigureJwt(builder.Services, configuration);
CustomServiceExtensions.ConfigureSwagger(builder.Services);

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

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}