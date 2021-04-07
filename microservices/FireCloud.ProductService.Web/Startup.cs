using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using Shared.Infrastructure.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
=======
using System;
using System.Collections.Generic;
using System.Linq;
>>>>>>> c7ef014e71224f4e3f1913488e41e3b7fd27e353
using System.Threading.Tasks;

namespace FireCloud.ProductService.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
<<<<<<< HEAD
            //×¢ÈëElasticSearch
            services.Configure<EsConfig>(option =>
            {
                option.Urls = Configuration.GetSection("EsConfig:ConnectionStrings").GetChildren().ToList().Select(p => p.Value).ToList();
            });
            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            var type = Assembly.Load("").GetTypes().Where(p => !p.IsAbstract && (p.GetInterfaces().Any(i => i == typeof(IBaseEsContext)))).ToList();
            type.ForEach(p => services.AddTransient(p));

=======
>>>>>>> c7ef014e71224f4e3f1913488e41e3b7fd27e353
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
