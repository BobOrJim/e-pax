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

namespace IDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Hi. First thing first weary traveler and friend. I hope you drop me an email so i can buy you a cold beer in Borås. Jimmy.Nordin.1979@gmail.com

            //To get access to DI services before the host is started.
            var host = CreateHostBuilder(args).Build();


            //NOTE: Claims and/or Roles can be used with Core-Identity becouse is4 can work with both.
            //As a personal preference ive decided to use only roles.
            using (var scope = host.Services.CreateScope())
            {
            }

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
