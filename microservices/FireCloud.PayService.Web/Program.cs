using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
<<<<<<< HEAD
using Shared.Infrastructure.Serilog;
using System;
=======
>>>>>>> c7ef014e71224f4e3f1913488e41e3b7fd27e353

namespace FireCloud.PayService.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
<<<<<<< HEAD
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
=======
            CreateHostBuilder(args).Build().Run();
>>>>>>> c7ef014e71224f4e3f1913488e41e3b7fd27e353
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
