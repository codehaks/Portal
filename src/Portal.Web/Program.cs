using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Portal.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();

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
