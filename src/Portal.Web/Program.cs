using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Portal.Domain;
using Portal.Persistence;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Portal.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Length == 1 && args[0] == "init")
            {

                using (var scope = host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var ensureCreated = await dbContext.Database.EnsureCreatedAsync();

                    if (ensureCreated)
                    {
                        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                        var seedIdentity = new SeedIdentityData(userManager, roleManager);
                        await seedIdentity.Init();
                    }
                }

                Console.Write("Seeding users completed.");
            }
            else
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSerilog((webHostBuilderContext, logger) =>
                    {
                        if (webHostBuilderContext.HostingEnvironment.IsProduction())
                        {
                            logger.WriteTo.MSSqlServer(
                                webHostBuilderContext
                                .Configuration
                                .GetSection("Logging:mssql").Value,
                                  sinkOptions: new MSSqlServerSinkOptions
                                  {
                                      TableName = "Logs",
                                      AutoCreateSqlTable = true
                                  }
                                ).MinimumLevel.Error();
                        }
                        else
                        {
                            logger.WriteTo.Console().MinimumLevel.Information();
                        }
                    });
                });
    }
}
