using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Service.Endpoint;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomDbContext(builder.Configuration)
                //.AddCustomIdentity()
                .AddCustomJwtAuthentication(builder.Configuration)
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddCustomValidators()
                .AddCustomMediatR()
                .AddControllers()
                .Services.AddCustomSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();