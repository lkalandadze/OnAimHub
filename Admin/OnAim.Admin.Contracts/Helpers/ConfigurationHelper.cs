﻿////using Microsoft.Extensions.Configuration;

//namespace OnAim.Admin.Contracts.Helpers;

//public static class ConfigurationHelper
//{
//    public static IConfiguration GetConfiguration(string basePath = null)
//    {
//        basePath ??= Directory.GetCurrentDirectory();
//        var builder = new ConfigurationBuilder()
//            .SetBasePath(basePath)
//            .AddJsonFile("appsettings.json")
//            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
//            .AddEnvironmentVariables();

//        return builder.Build();
//    }
//}
