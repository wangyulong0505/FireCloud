using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.Serilog;
using System;

namespace FireCloud.PayService.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = SerilogExtensions.ImportSerilog("log.txt");
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                //Serilog.Log.Fatal(ex, "Stopped program because of exception");
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                SerilogExtensions.SerilogDispose();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
