using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MetricsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger nLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                nLogger.Debug("NLogeer initilized in 'Main'");

                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception e)
            {
                nLogger.Error(e, "Application terminated unexpectdly");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(logging =>
                    {
                        //To remove or default logging providers
                        logging.ClearProviders();

                        //To set the log level for recording in file and console
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

                        //Enable logging to console.
                        logging.AddConsole();

                    })
                    .UseNLog()
                    .UseStartup<Startup>();
                });
    }
}
