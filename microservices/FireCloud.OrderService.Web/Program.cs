using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;
using System;

namespace FireCloud.OrderService.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogExtensions.ImportNlog();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLogExtensions.NlogDispose();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).AddNlog();
    }
}
