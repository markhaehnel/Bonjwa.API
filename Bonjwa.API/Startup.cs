using System;
using System.IO;
using System.Net.Http;
using Bonjwa.API.Config;
using Bonjwa.API.Services;
using Bonjwa.API.Storage;
using Bonjwa.API.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Bonjwa.API
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
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new Converters.DateTimeConverter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bonjwa.API", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddSingleton<IDataStore, InMemoryDataStore>();
            services.AddSingleton<IFetchService, HttpFetchService>();
            services.AddSingleton(typeof(BonjwaScrapeService));
            services.AddHostedService<BonjwaScrapeTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 404 catch all
            app.Use(async (ctx, next) =>
            {
                await next().ConfigureAwait(false);
                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    ctx.Request.Path = "/404";
                    await next().ConfigureAwait(false);
                }
            });

            // Swagger
            app.UseSwagger(c => c.RouteTemplate = $"{AppConfig.ApiDocsPath}/{{documentName}}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = AppConfig.ApiDocsPath;
                c.SwaggerEndpoint($"/{AppConfig.ApiDocsPath}/v1/swagger.json", "Bonjwa.API v1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
