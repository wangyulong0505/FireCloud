using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Shared.Infrastructure
{
    public static class ElasticSearchServiceExtension
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration Configuration, string assemblyName)
        {
            services.Configure<EsConfig>(option =>
            {
                option.Urls = Configuration.GetSection("EsConfig:ConnectionStrings").GetChildren().ToList().Select(p => p.Value).ToList();
            });
            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            var type = Assembly.Load(assemblyName).GetTypes().Where(p => !p.IsAbstract && (p.GetInterfaces().Any(i => i == typeof(IBaseEsContext)))).ToList();
            type.ForEach(p => services.AddTransient(p));

            return services;
        }
    }
}
