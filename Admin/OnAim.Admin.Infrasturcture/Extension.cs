using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Persistance.Data.Hub;
using OnAim.Admin.Infrasturcture.Persistance.Data.LeaderBoard;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;

namespace OnAim.Admin.Infrasturcture;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var configuration = context.Resolve<IConfiguration>();
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));
            return new DatabaseContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        builder.Register(context =>
        {
            var configuration = context.Resolve<IConfiguration>();
            var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyDataContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("HubDefaultConnectionString"));
            return new ReadOnlyDataContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        builder.Register(context =>
        {
            var configuration = context.Resolve<IConfiguration>();
            var optionsBuilder = new DbContextOptionsBuilder<LeaderBoardReadOnlyDataContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("LeaderBoardDefaultConnectionString"));
            return new LeaderBoardReadOnlyDataContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        builder.Register(context =>
        {
            var configuration = context.Resolve<IConfiguration>();
            var mongoOptions = configuration.GetSection("MongoDB").Get<MongoDbOptions>();
            return Options.Create(mongoOptions);
        }).As<IOptions<MongoDbOptions>>().SingleInstance();

        builder.RegisterType<AuditLogDbContext>().InstancePerLifetimeScope();
    }
}


//public static class Extension
//{
//    private static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
//    {
//        var options = configuration.GetSection("MongoDB");
//        services.AddScoped<AuditLogDbContext>().AddOptions<MongoDbOptions>().Bind(options);
//    }
//}
