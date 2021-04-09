using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Shared.Infrastructure
{
    public static class AppMetricsServiceExtension
    {
        public static IServiceCollection AddAppMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            bool isOpenMetrics = Convert.ToBoolean(configuration["AppMetrics:IsOpen"]);
            if(isOpenMetrics)
            {
                string database = configuration["AppMetrics:DatabaseName"];
                string connStr = configuration["AppMetrics:ConnectionString"];
                string app = configuration["AppMetrics:App"];
                string env = configuration["AppMetrics:Env"];
                string username = configuration["AppMetrics:UserName"];
                string password = configuration["AppMetrics:Password"];
                var uri = new Uri(connStr);
                var metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
                {
                    options.AddAppTag(app);
                    options.AddEnvTag(env);
                }).Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = uri;
                    options.InfluxDb.Database = database;
                    options.InfluxDb.UserName = username;
                    options.InfluxDb.Password = password;
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.FlushInterval = TimeSpan.FromSeconds(5);
                }).Build();

                services.AddMetrics(metrics);
                services.AddMetricsReportingHostedService();
                services.AddMetricsTrackingMiddleware();
                services.AddMetricsEndpoints();
            }
            return services;
        }

        public static IApplicationBuilder UseAppMetrics(this IApplicationBuilder app, IConfiguration configuration)
        {
            bool isOpenMetrics = Convert.ToBoolean(configuration["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();
            }

            return app;
        }
    }
}
