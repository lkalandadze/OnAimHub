﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.Infrasturcture.Persistance.Data;

namespace OnAim.Admin.Infrasturcture
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));
            });

            return services;
        }
    }
}