using OnAim.Admin.API.Extensions;
using OnAim.Admin.API.Factory;
using OnAim.Admin.API.Middleware;
using OnAim.Admin.APP;
using OnAim.Admin.Identity;
using OnAim.Admin.Identity.Entities;
using OnAim.Admin.Identity.Persistance;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture;
using OnAim.Admin.Infrasturcture.Repository;
using OnAim.Admin.Infrasturcture.Repository.Abstract;


var builder = WebApplication.CreateBuilder(args);

builder.Services
                //.AddCustomIdentity()
                .AddCustomJwtAuthentication(builder.Configuration)
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddControllers()
                .Services.AddCustomSwagger();

builder.Services.AddIdentityServerAuthentication<ApplicationIdentityDbContext, User, ApplicationUserManager>(builder.Configuration);

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(ApplicationContextFactory.Create);
builder.Services.AddApp();
builder.Services.AddInfrastructure(builder.Configuration);

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

app.UseMiddleware<PermissionMiddleware>();
//app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
