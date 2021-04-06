using Serilog;
using Serilog.Core;

namespace Shared.Infrastructure.Serilog
{
    public static class SerilogExtensions
    {
        public static Logger ImportSerilog(string path)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        public static void SerilogDispose()
        {
            Log.CloseAndFlush();
        }
    }
}
