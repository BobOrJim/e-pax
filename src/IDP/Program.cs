using IDP.Entities;
using IDP.DBContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;
using System.IO;

namespace IDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Hi. First thing first weary traveler and friend. I hope you drop me an email so i can buy you a cold beer in Borås. Jimmy.Nordin.1979@gmail.com


            //To get access to DI services before the host is started.
            var host = CreateHostBuilder(args).Build();


            var currentDirectory = Directory.GetCurrentDirectory();
            var logDirectory = Directory.GetParent(currentDirectory.ToString()) + "/Logs/IDP_Locallog_.txt";
            Console.WriteLine(logDirectory);

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logDirectory, rollingInterval: RollingInterval.Day) //\DashboardServer.log
            .CreateLogger();
            //Log.Information("logDirectory = " + logDirectory.ToString());


            Log.Information("Hello, from program.cs in IDP");

            using (var scope = host.Services.CreateScope()){}

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
