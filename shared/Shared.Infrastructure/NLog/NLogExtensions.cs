using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;


namespace Shared.Infrastructure.NLog
{
    public static class NLogExtensions
    {
        public static IHostBuilder AddNlog(this IHostBuilder builder)
        {
            return builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            }).UseNLog();
        }

        public static Logger ImportNlog()
        {
            return NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        }

        public static void NlogDispose()
        {
            LogManager.Shutdown();
        }
    }
}
